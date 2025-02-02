using System;
using System.Windows;
using System.Windows.Controls;

namespace InterfaceForDB
{
    public partial class CreateUserRideWindow : Window
    {
        private readonly RideService _rideService;
        private int CustomerId;
        private Rate selectedTariff;

        public CreateUserRideWindow(int customerId, RideService rideService)
        {
            InitializeComponent();
            CustomerId = customerId;
            _rideService = rideService;
            LoadTariffs();
        }

        private void LoadTariffs()
        {
            try
            {
                var tariffs = _rideService.GetAllTariffs();

                if (tariffs.Count > 0)
                {
                    foreach (var tariff in tariffs)
                    {
                        var listBoxItem = new ListBoxItem
                        {
                            Content = $"{tariff.type} - {tariff.Base_fare} грн",
                            Tag = tariff
                        };
                        TariffListBox.Items.Add(listBoxItem);
                    }
                }
                else
                {
                    MessageBox.Show("Тарифи не знайдено в базі даних.", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час завантаження тарифів: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TariffListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TariffListBox.SelectedItem != null)
            {
                var selectedItem = (ListBoxItem)TariffListBox.SelectedItem;
                selectedTariff = (Rate)selectedItem.Tag;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PickupLocationTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DropoffLocationTextBox.Text) ||
                    selectedTariff == null)
                {
                    MessageBox.Show("Будь ласка, заповніть всі поля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _rideService.CreateRide(CustomerId, PickupLocationTextBox.Text, DropoffLocationTextBox.Text, selectedTariff);

                MessageBox.Show("Поїздка успішно створена!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
