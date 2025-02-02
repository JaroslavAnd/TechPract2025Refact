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
    /// Interaction logic for AuditLogPage.xaml
    /// </summary>
    public partial class AuditLogPage : Page
    {
        TaxiServiceContext db = new TaxiServiceContext();

        public AuditLogPage()
        {
            InitializeComponent();
            LoadAuditLog();
        }

        private void LoadAuditLog()
        {
            AuditLogGrid.ItemsSource = db.AuditLog.ToList();
        }
    }
}
