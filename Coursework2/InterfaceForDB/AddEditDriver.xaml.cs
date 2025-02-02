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
    /// Interaction logic for AddEditDriver.xaml
    /// </summary>
    public partial class AddEditDriverWindow : Window
    {
        private Driver Driver;

        public AddEditDriverWindow(Driver driver = null)
        {
            InitializeComponent();
            Driver = driver ?? new Driver();

            if (driver != null)
            {
                NameTextBox.Text = driver.Name;
                PhoneTextBox.Text = driver.Phone;
                LicenseTextBox.Text = driver.License_number;
                RatingTextBox.Text = driver.Rating.ToString();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Driver.Name = NameTextBox.Text;
            Driver.Phone = PhoneTextBox.Text;
            Driver.License_number = LicenseTextBox.Text;
            Driver.Rating = decimal.Parse(RatingTextBox.Text);

            try
            {
                using (var db = new TaxiServiceContext())
                {
                    if (Driver.Id == 0)
                    {
                        db.Drivers.Add(Driver); 
                    }
                    else
                    {
                        db.Entry(Driver).State = System.Data.Entity.EntityState.Modified; 
                    }

                    db.SaveChanges();
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

           
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
