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
    /// Interaction logic for CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : Page
    {
        TaxiServiceContext db = new TaxiServiceContext(); 
        public CustomersPage()
        {
            InitializeComponent();
            LoadCustomers();
        }
        private void LoadCustomers()
        {
            CustomersGrid.ItemsSource = db.Customers.ToList();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditCustomerWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadCustomers();
            }
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersGrid.SelectedItem is Customer selectedCustomer)
            {
                var editWindow = new AddEditCustomerWindow(selectedCustomer);
                if (editWindow.ShowDialog() == true)
                {
                    LoadCustomers();
                }
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersGrid.SelectedItem is Customer selectedCustomer)
            {
                db.Customers.Remove(selectedCustomer);
                db.SaveChanges();
                LoadCustomers();
            }
        }
    }
}
