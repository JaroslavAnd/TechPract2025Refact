using System;
using System.Collections.Generic;
using System.Data;
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
using System.Data.SqlClient;
using Microsoft.VisualBasic;

namespace InterfaceForDB
{
    /// <summary>
    /// Interaction logic for ProcedureWindow.xaml
    /// </summary>
    public partial class ProcedureWindow : Window
    {
        private string connectionString = "Server=DESKTOP-6SGVU8V\\MSSQLSERVER15;Database=TaxiService;Trusted_Connection=True;";
        public ProcedureWindow()
        {
            InitializeComponent();
        }
        private void ExecuteProcedure(string procedureName, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        ResultsDataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка виконання процедури", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetDriverRides_Click(object sender, RoutedEventArgs e)
        {
            var input = Microsoft.VisualBasic.Interaction.InputBox("Введіть ID водія:", "Поїздки водія", "1");
            if (int.TryParse(input, out int driverId))
            {
                ExecuteProcedure("GetDriverRides", new[] { new SqlParameter("@driver_id", driverId) });
            }
            else
            {
                MessageBox.Show("Невірний ID водія!");
            }
        }

        private void GetPaymentsByDateRange_Click(object sender, RoutedEventArgs e)
        {
            string startDate = Microsoft.VisualBasic.Interaction.InputBox("Введіть початкову дату (yyyy-MM-dd):", "Платежі за період", "2024-01-01");
            string endDate = Microsoft.VisualBasic.Interaction.InputBox("Введіть кінцеву дату (yyyy-MM-dd):", "Платежі за період", "2024-12-31");

            ExecuteProcedure("GetPaymentsByDateRange", new[]
            {
                new SqlParameter("@start_date", startDate),
                new SqlParameter("@end_date", endDate)
            });
        }

        private void GetCustomerRideCount_Click(object sender, RoutedEventArgs e)
        {
            ExecuteProcedure("GetCustomerRideCount");
        }

        

        private void GetSystemActivityReport_Click(object sender, RoutedEventArgs e)
        {
            string startDate = Microsoft.VisualBasic.Interaction.InputBox("Введіть початкову дату (yyyy-MM-dd, опціонально):", "Звіт активності", "");
            string endDate = Microsoft.VisualBasic.Interaction.InputBox("Введіть кінцеву дату (yyyy-MM-dd, опціонально):", "Звіт активності", "");
            string eventType = Microsoft.VisualBasic.Interaction.InputBox("Введіть тип події (опціонально):", "Звіт активності", "");

            ExecuteProcedure("GetSystemActivityReport", new[]
            {
                new SqlParameter("@start_date", string.IsNullOrWhiteSpace(startDate) ? (object)DBNull.Value : startDate),
                new SqlParameter("@end_date", string.IsNullOrWhiteSpace(endDate) ? (object)DBNull.Value : endDate),
                new SqlParameter("@event_type", string.IsNullOrWhiteSpace(eventType) ? (object)DBNull.Value : eventType),
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
