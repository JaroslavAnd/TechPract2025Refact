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
    /// Interaction logic for AddEditPaymentWindow.xaml
    /// </summary>
    public partial class AddEditPaymentWindow : Window
    {
        private Payment Payment;

        public AddEditPaymentWindow(Payment payment = null)
        {
            InitializeComponent();
            Payment = payment ?? new Payment();

            if (payment != null)
            {
                RideIdTextBox.Text = payment.Ride_id.ToString();
                AmountTextBox.Text = payment.Amount.ToString();
                PaymentMethodComboBox.Text = payment.Payment_method;
                PaymentDatePicker.SelectedDate = payment.Payment_date;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(RideIdTextBox.Text, out int rideId) || !decimal.TryParse(AmountTextBox.Text, out decimal amount))
            {
                MessageBox.Show("Invalid Ride ID or Amount.");
                return;
            }

            Payment.Ride_id = rideId;
            Payment.Amount = amount;
            Payment.Payment_method = PaymentMethodComboBox.Text;
            Payment.Payment_date = PaymentDatePicker.SelectedDate ?? DateTime.Now;

            try
            {
                using (var db = new TaxiServiceContext())
                {
                    if (Payment.Id == 0)
                    {
                        db.Payments.Add(Payment); 
                    }
                    else
                    {
                        db.Entry(Payment).State = System.Data.Entity.EntityState.Modified; 
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
