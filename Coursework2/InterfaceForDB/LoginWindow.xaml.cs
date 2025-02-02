using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new TaxiServiceContext())
            {
                string username = txtUsername.Text;
                string password = txtPassword.Password;

                var user = db.Users.FirstOrDefault(u => u.Username == username);
              
                if (user != null && VerifyPassword(password, user.PasswordHash))
                {
                 
                    if (user.Role == "Admin")
                    {
                        MainWindow adminWindow = new MainWindow();
                        adminWindow.Show();
                    }
                    else if (user.Role == "User")
                    {
                        UserWindow userWindow = new UserWindow(user.Id);
                        userWindow.Show();
                    }

                    Close();
                }
                else
                {
                    txtErrorMessage.Text = "Невірний логін або пароль.";
                }
            }
        }
        private bool VerifyPassword(string password, string storedHash)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                string hash = BitConverter.ToString(hashedBytes).Replace("-", "");
                
                return hash == storedHash;
            }
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Hide();
        }
    }
}
