using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace InterfaceForDB
{
    /// <summary>
    /// Interaction logic for CreateUserRideWindow.xaml
    public partial class CreateUserRideWindow : Window
    {
        private int CustomerId;
        private Rate selectedTariff;

        public CreateUserRideWindow(int customerId)
        {
            InitializeComponent();
            CustomerId = customerId;
            LoadTariffs();
        }

        private void LoadTariffs()
        {
            try
            {
                using (var db = new TaxiServiceContext())
                {
                   
                    var tariffs = db.Rates.ToList();

                 
                    if (tariffs.Any())
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
                        System.Windows.MessageBox.Show("Не знайдено тарифів у базі даних.", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Сталася помилка при завантаженні тарифів: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    System.Windows.MessageBox.Show("Будь ласка, заповніть всі поля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var db = new TaxiServiceContext())
                {
                    db.Database.Log = Console.WriteLine;
                    var driver = db.Drivers.FirstOrDefault(d => !db.Rides.Any(r => r.Driver_id == d.Id));
                    if (driver == null)
                    {
                        System.Windows.MessageBox.Show("Немає доступних водіїв для створення поїздки.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                   
                    var ride = new Ride
                    {
                        Pickup_location = PickupLocationTextBox.Text,
                        Dropoff_location = DropoffLocationTextBox.Text,
                        Ride_date = DateTime.Now,
                        Fare = selectedTariff.Base_fare, 
                        Customer_id = CustomerId,
                        Driver_id = driver.Id,
                        Rate_id = selectedTariff.Id 
                    };
                    Console.WriteLine($"Додати поїздку з тарифом {selectedTariff.type} і водієм {driver.Name}");
                    db.Rides.Add(ride);
                    db.SaveChanges();

                    System.Windows.MessageBox.Show("Поїздка успішно створена!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні тарифів: {ex.Message}");
                System.Windows.MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

}
