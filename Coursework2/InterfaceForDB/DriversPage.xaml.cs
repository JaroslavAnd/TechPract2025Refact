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
    /// Interaction logic for DriversPage.xaml
    /// </summary>
    public partial class DriversPage : Page
    {
        TaxiServiceContext db = new TaxiServiceContext();
        public DriversPage()
        {
            InitializeComponent();
            LoadDrivers();
        }
        private void LoadDrivers()
        {
            DriversGrid.ItemsSource = db.Drivers.ToList();
        }

        private void AddDriver_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditDriverWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadDrivers();
            }
        }

        private void EditDriver_Click(object sender, RoutedEventArgs e)
        {
            if (DriversGrid.SelectedItem is Driver selectedDriver)
            {
                var editWindow = new AddEditDriverWindow(selectedDriver);
                if (editWindow.ShowDialog() == true)
                {
                    LoadDrivers();
                }
            }
        }

        private void DeleteDriver_Click(object sender, RoutedEventArgs e)
        {
            if (DriversGrid.SelectedItem is Driver selectedDriver)
            {
                db.Drivers.Remove(selectedDriver);
                db.SaveChanges();
                LoadDrivers();
            }
        }
    }
}
