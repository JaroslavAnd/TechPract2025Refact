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
    /// Interaction logic for AddEditRateWindow.xaml
    /// </summary>
    public partial class AddEditRateWindow : Window
    {
        private Rate Rate;

        public AddEditRateWindow(Rate rate = null)
        {
            InitializeComponent();
            Rate = rate ?? new Rate();

            if (rate != null)
            {
                BaseFareTextBox.Text = rate.Base_fare.ToString();
                CostPerKmTextBox.Text = rate.Per_km_rate.ToString();
                CostPerMinuteTextBox.Text = rate.Per_min_rate.ToString();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Rate.Base_fare = decimal.Parse(BaseFareTextBox.Text);
            Rate.Per_km_rate = decimal.Parse(CostPerKmTextBox.Text);
            Rate.Per_min_rate = decimal.Parse(CostPerMinuteTextBox.Text);

            try
            {
                using (var db = new TaxiServiceContext())
                {
                    if (Rate.Id == 0)
                    {
                        db.Rates.Add(Rate); 
                    }
                    else
                    {
                        db.Entry(Rate).State = System.Data.Entity.EntityState.Modified; 
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
