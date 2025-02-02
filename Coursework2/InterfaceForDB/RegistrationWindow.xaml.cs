using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace InterfaceForDB
{
   
    public partial class RegistrationWindow : Window
    {
        private User User { get; set; }
        private Customer Customer { get; set; }

        public RegistrationWindow()
        {
            InitializeComponent();

            User = new User();
            Customer = new Customer();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes).ToUpper();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            
            User.Username = UsernameTextBox.Text.Trim();
            User.PasswordHash = HashPassword(PasswordBox.Password.Trim());
            User.phone_number = PhoneTextBox.Text.Trim();
            User.Role = "User";

            if (string.IsNullOrEmpty(User.Username) || string.IsNullOrEmpty(User.PasswordHash) || string.IsNullOrEmpty(User.phone_number))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var db = new TaxiServiceContext())
                {
                    db.Database.Log = Console.WriteLine;
                    if (db.Users.Any(u => u.Username == User.Username))
                    {
                        MessageBox.Show("A user with this username already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    db.Users.Add(User);
                    db.SaveChanges();  

                    User = db.Users.FirstOrDefault(u => u.Username == User.Username);

                    CreateCustomer(db);
                }
            }
            catch (Exception ex)
            {
              
                MessageBox.Show($"An error occurred: {ex.Message}\nInner Exception: {ex.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateCustomer(TaxiServiceContext db)
        {
            Customer.Name = NameTextBox.Text.Trim();
            Customer.Phone = PhoneTextBox.Text.Trim();
            Customer.Email = EmailTextBox.Text.Trim();
            Customer.Address = "GeoLocation";  

            if (string.IsNullOrEmpty(Customer.Email))
            {
                MessageBox.Show("Please provide an email for the customer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Customer.User_id = User.Id;

                if (db.Customers.Any(c => c.Email == Customer.Email))
                {
                    MessageBox.Show("A customer with this email already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                db.Customers.Add(Customer);
                db.SaveChanges();

                MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                UsernameTextBox.Clear();
                PasswordBox.Clear();
                EmailTextBox.Clear();
                PhoneTextBox.Clear();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"An error occurred while creating customer: {ex.Message}\nInner Exception: {ex.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
         
            this.Close();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
