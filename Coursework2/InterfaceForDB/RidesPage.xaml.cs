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
    /// Interaction logic for RidesPage.xaml
    /// </summary>
    public partial class RidesPage : Page
    {
        TaxiServiceContext db = new TaxiServiceContext();

        public RidesPage()
        {
            InitializeComponent();
            LoadRides();
        }

        private void LoadRides()
        {
            RidesGrid.ItemsSource = db.Rides.ToList();
        }

        private void AddRide_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditRideWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadRides();
            }
        }

       

        private void DeleteRide_Click(object sender, RoutedEventArgs e)
        {
            if (RidesGrid.SelectedItem is Ride selectedRide)
            {
                db.Rides.Remove(selectedRide);
                    db.SaveChanges();
                LoadRides();
            }
        }
    }
}
