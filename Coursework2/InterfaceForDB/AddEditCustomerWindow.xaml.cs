using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.VisualBasic;

namespace InterfaceForDB
{
    public partial class AddEditCustomerWindow : Window
    {
        private Customer Customer;

        public AddEditCustomerWindow(Customer customer = null)
        {
            InitializeComponent();
            Customer = customer ?? new Customer();

            if (customer != null)
            {
                NameTextBox.Text = customer.Name;
                PhoneTextBox.Text = customer.Phone;
                EmailTextBox.Text = customer.Email;
                AddressTextBox.Text = customer.Address;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Customer.Name = NameTextBox.Text;
            Customer.Phone = PhoneTextBox.Text;
            Customer.Email = EmailTextBox.Text;
            Customer.Address = AddressTextBox.Text;
            try
            {
                using (var db = new TaxiServiceContext())
                {
                    db.Database.Log = Console.WriteLine;
                    var existingUser = db.Users.FirstOrDefault(u => u.phone_number == Customer.Phone);

                    if (existingUser != null)
                    {
                        Customer.User_id = existingUser.Id;
                    }
                    else
                    {
                        
                        string password = Interaction.InputBox("Please enter a password for the new user:", "Enter Password", "", -1, -1);

                        if (string.IsNullOrEmpty(password))
                        {
                            MessageBox.Show("Password is required!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var newUser = new User
                        {
                            Username = Customer.Email,
                            PasswordHash = GetPasswordHash(password),  
                            phone_number = Customer.Phone,
                            Role = "User"
                        };

                        db.Users.Add(newUser);
                        db.SaveChanges();

                        Customer.User_id = newUser.Id;
                    }

                    if (Customer.Id == 0)
                    {
                        db.Customers.Add(Customer);
                    }
                    else
                    {
                        var existingCustomer = db.Customers.Find(Customer.Id);
                        if (existingCustomer != null)
                        {
                            existingCustomer.Name = NameTextBox.Text;
                            existingCustomer.Phone = PhoneTextBox.Text;
                            existingCustomer.Email = EmailTextBox.Text;
                            existingCustomer.Address = AddressTextBox.Text;

                            db.Entry(existingCustomer).State = System.Data.Entity.EntityState.Modified;
                        }
                    }

                    int changes = db.SaveChanges();
                    if (changes > 0)
                    {
                        MessageBox.Show("Customer saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("No changes were made to the database.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}\nInner Exception: {ex.InnerException?.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private string GetPasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashedPassword = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedPassword);
            }
        }
    }
}
