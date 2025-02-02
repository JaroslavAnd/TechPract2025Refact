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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InterfaceForDB
{
    /// <summary>
    /// Interaction logic for VehiclesPage.xaml
    /// </summary>
    public partial class VehiclesPage : Page
    {
        TaxiServiceContext db = new TaxiServiceContext();

        public VehiclesPage()
        {
            InitializeComponent();
            LoadVehicles();
        }

        private void LoadVehicles()
        {
            VehiclesGrid.ItemsSource = db.Vehicles.ToList();
        }

        private void AddVehicle_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditVehicleWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadVehicles();
            }
        }

        private void EditVehicle_Click(object sender, RoutedEventArgs e)
        {
            if (VehiclesGrid.SelectedItem is Vehicle selectedVehicle)
            {
                var editWindow = new AddEditVehicleWindow(selectedVehicle);
                if (editWindow.ShowDialog() == true)
                {
                    LoadVehicles();
                }
            }
        }

        private void DeleteVehicle_Click(object sender, RoutedEventArgs e)
        {
            if (VehiclesGrid.SelectedItem is Vehicle selectedVehicle)
            {
                db.Vehicles.Remove(selectedVehicle);
                db.SaveChanges();
                LoadVehicles();
            }
        }
    }
}
