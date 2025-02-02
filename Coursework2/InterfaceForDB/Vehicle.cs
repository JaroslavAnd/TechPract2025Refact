using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceForDB
{
    public class Vehicle
    {
        public int Id { get; set; } 
        public int Driver_id { get; set; } 
        public string Make { get; set; }
        public string Model { get; set; } 
        public string License_plate { get; set; } 
        public int Year { get; set; } 
        public int Rate_id { get; set; }
        public virtual Driver Driver { get; set; } 
    }
}
