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
    /// Interaction logic for PaymentsPage.xaml
    /// </summary>
    public partial class PaymentsPage : Page
    {
        TaxiServiceContext db = new TaxiServiceContext();

        public PaymentsPage()
        {
            InitializeComponent();
            LoadPayments();
        }

        private void LoadPayments()
        {
            PaymentsGrid.ItemsSource = db.Payments.ToList();
        }

        private void AddPayment_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditPaymentWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadPayments();
            }
        }

        private void EditPayment_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentsGrid.SelectedItem is Payment selectedPayment)
            {
                var editWindow = new AddEditPaymentWindow(selectedPayment);
                if (editWindow.ShowDialog() == true)
                {
                    LoadPayments();
                }
            }
        }

        private void DeletePayment_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentsGrid.SelectedItem is Payment selectedPayment)
            {
                db.Payments.Remove(selectedPayment);
                db.SaveChanges();
                LoadPayments();
            }
        }
    }
}
