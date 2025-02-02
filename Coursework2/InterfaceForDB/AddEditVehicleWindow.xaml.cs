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
    /// Interaction logic for AddEditVehicleWindow.xaml
    /// </summary>
   
        public partial class AddEditVehicleWindow : Window
        {
            private Vehicle Vehicle;

            public AddEditVehicleWindow(Vehicle vehicle = null)
            {
                InitializeComponent();
                Vehicle = vehicle ?? new Vehicle();

                if (vehicle != null)
                {
                    ModelTextBox.Text = vehicle.Model;
                    LicensePlateTextBox.Text = vehicle.License_plate;
                    YearTextBox.Text = vehicle.Year.ToString();
                    MarkTextBox.Text = vehicle.Make;
                    DriverTextBox.Text = vehicle.Driver_id.ToString();
                    RateTextBox.Text = vehicle.Rate_id.ToString();
                }
            }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Vehicle.Model = ModelTextBox.Text;
            Vehicle.License_plate = LicensePlateTextBox.Text;
            Vehicle.Make = MarkTextBox.Text;
            Vehicle.Driver_id =int.Parse(DriverTextBox.Text);
            Vehicle.Rate_id =int.Parse(RateTextBox.Text);
            if (!int.TryParse(YearTextBox.Text.Trim(), out var year))
            {
                MessageBox.Show("Year must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Vehicle.Year = year;

            try
            {
                using (var db = new TaxiServiceContext())
                {
                    db.Database.Log = Console.WriteLine;
                    if (Vehicle.Id == 0) 
                    {
                        
                        if (db.Vehicles.Any(v => v.License_plate == Vehicle.License_plate))
                        {
                            MessageBox.Show("A vehicle with the same license plate already exists.",
                                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        db.Vehicles.Add(Vehicle);
                    }
                    else 
                    {
                        var existingVehicle = db.Vehicles.Find(Vehicle.Id);
                        if (existingVehicle != null)
                        {
                            existingVehicle.Model = Vehicle.Model;
                            existingVehicle.License_plate = Vehicle.License_plate;
                            existingVehicle.Year = Vehicle.Year;
                            existingVehicle.Make = Vehicle.Make;
                            existingVehicle.Driver_id = Vehicle.Driver_id;
                            existingVehicle.Rate_id = Vehicle.Rate_id;
                        }
                    }

                    db.SaveChanges();
                }

                MessageBox.Show("Vehicle saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
        }
    }

