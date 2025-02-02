using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceForDB
{
    public class Ride
    {
        public int Id { get; set; } 
        public int Customer_id { get; set; } 
        public int Driver_id { get; set; } 
        public string Pickup_location { get; set; } 
        public string Dropoff_location { get; set; } 
        public decimal Fare { get; set; } 
        public int Rate_id { get; set; }
        public DateTime Ride_date { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual Rate Rate { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

    }
}
