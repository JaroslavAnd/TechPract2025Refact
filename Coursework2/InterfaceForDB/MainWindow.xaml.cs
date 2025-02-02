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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MenuItem_Customers_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CustomersPage());
        }

        private void MenuItem_Drivers_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DriversPage());
        }

        private void MenuItem_Vehicles_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new VehiclesPage());
        }

        private void MenuItem_Rides_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RidesPage());
        }

        private void MenuItem_Rates_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RatesPage());
        }
       
        private void MenuItem_AuditLog_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AuditLogPage());
        }

        private void MenuItem_Payment_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaymentsPage());
        }
        private void MenuItem_Procedure_Click(object sender, RoutedEventArgs e)
        {
            ProcedureWindow procedureWindow = new ProcedureWindow();
            procedureWindow.Show();
            this.Close();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
