using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InterfaceForDB
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private int UserId;

        public UserWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;
            LoadUserData();
            LoadRides();
        }

        private void LoadUserData()
        {
            using (var db = new TaxiServiceContext())
            {
                
                var customer = db.Customers.FirstOrDefault(c => c.User_id == UserId);
                if (customer != null)
                {
                    
                    var user = db.Users.FirstOrDefault(u => u.Id == customer.User_id);
                    if (user != null)
                    {
                        WelcomeText.Text = $"Вітаємо, {user.Username}!";
                    }
                    else
                    {
                        WelcomeText.Text = "Не знайдено користувача!";
                    }
                }
                else
                {
                    WelcomeText.Text = "Не знайдено клієнта!";
                }
            }
        }


        private void LoadRides()
        {
            using (var db = new TaxiServiceContext())
            {
                
                var customer = db.Customers.FirstOrDefault(c => c.User_id == UserId);

                if (customer != null)
                {
                    var rides = db.Rides
                                  .Where(r => r.Customer_id == customer.Id)  
                                  .ToList();

                    if (rides.Count > 0)
                    {
                        RidesDataGrid.ItemsSource = rides;
                    }
                    else
                    {
                        
                        MessageBox.Show("Немає поїздок для цього користувача.");
                        RidesDataGrid.ItemsSource = null;
                    }
                }
                else
                {
                    MessageBox.Show("Клієнт не знайдений!");
                }
            }
        }


        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadRides();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

        private void CreateRideButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new TaxiServiceContext())
            {
                // Завантаження клієнта для перевірки
                var customer = db.Customers.FirstOrDefault(c => c.User_id == UserId);

                if (customer == null)
                {
                    MessageBox.Show("Клієнт не знайдений. Неможливо створити поїздку.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Відкриття вікна для створення нової поїздки
                var createRideWindow = new CreateUserRideWindow(customer.Id);
                if (createRideWindow.ShowDialog() == true)
                {
                    // Оновлення списку поїздок після успішного створення
                    LoadRides();
                }
            }
        }
    }
}
