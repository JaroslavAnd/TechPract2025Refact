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
    
    public partial class AddEditRideWindow : Window
    {
        private Ride Ride;

        public AddEditRideWindow(Ride ride = null)
        {
            InitializeComponent();
            Ride = ride ?? new Ride();

            using (var db = new TaxiServiceContext())
            {
                CustomerComboBox.ItemsSource = db.Customers.ToList();
                DriverComboBox.ItemsSource = db.Drivers.ToList();
            }

            if (ride != null)
            {
                CustomerComboBox.SelectedValue = ride.Customer_id;
                DriverComboBox.SelectedValue = ride.Driver_id;
                PickupLocationTextBox.Text = ride.Pickup_location;
                DropoffLocationTextBox.Text = ride.Dropoff_location;
                FareTextBox.Text = ride.Fare.ToString();
                RideDatePicker.SelectedDate = ride.Ride_date;
                RateTextBox.Text = ride.Rate.ToString();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerComboBox.SelectedValue == null || DriverComboBox.SelectedValue == null)
            {
                MessageBox.Show("Please select a customer and a driver.");
                return;
            }

            Ride.Customer_id = (int)CustomerComboBox.SelectedValue;
            Ride.Driver_id = (int)DriverComboBox.SelectedValue;
            Ride.Pickup_location = PickupLocationTextBox.Text;
            Ride.Dropoff_location = DropoffLocationTextBox.Text;
            Ride.Fare = decimal.TryParse(FareTextBox.Text, out var fare) ? fare : 0;
            Ride.Ride_date = RideDatePicker.SelectedDate ?? DateTime.Now;
            Ride.Rate_id= int.Parse(RateTextBox.Text);

            using (var db = new TaxiServiceContext())
            {
                
                if (Ride.Id == 0)
                {
                    db.Rides.Add(Ride);
                }
                else
                {
                    
                    db.Entry(Ride).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
                db.Database.Log = Console.WriteLine;
                var payment = new Payment
                {
                    
                    Ride_id = Ride.Id,
                    Amount = Ride.Fare,
                    Payment_method = "Готівка",
                    Payment_date = DateTime.Now
                };

                db.Payments.Add(payment);

               
                db.SaveChanges();
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
