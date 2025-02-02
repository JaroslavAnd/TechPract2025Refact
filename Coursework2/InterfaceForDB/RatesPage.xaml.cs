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
    /// Interaction logic for RatesPage.xaml
    /// </summary>
    public partial class RatesPage : Page
    {
        TaxiServiceContext db = new TaxiServiceContext();

        public RatesPage()
        {
            InitializeComponent();
            LoadRates();
        }

        private void LoadRates()
        {
            RatesGrid.ItemsSource = db.Rates.ToList();
        }

        private void AddRate_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditRateWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadRates();
            }
        }

        private void EditRate_Click(object sender, RoutedEventArgs e)
        {
            if (RatesGrid.SelectedItem is Rate selectedRate)
            {
                var editWindow = new AddEditRateWindow(selectedRate);
                if (editWindow.ShowDialog() == true)
                {
                    LoadRates();
                }
            }
        }

        private void DeleteRate_Click(object sender, RoutedEventArgs e)
        {
            if (RatesGrid.SelectedItem is Rate selectedRate)
            {
                db.Rates.Remove(selectedRate);
                db.SaveChanges();
                LoadRates();
            }
        }
    }
}
