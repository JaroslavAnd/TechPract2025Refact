using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceForDB
{
    public class Payment
    {
        public int Id { get; set; }
        public int Ride_id { get; set; }
        public decimal Amount { get; set; }
        public string Payment_method { get; set; }
        public DateTime Payment_date { get; set; }

        public virtual Ride Ride { get; set; }
    }
}
