using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceForDB
{
    public class Customer
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Phone { get; set; } 
        public string Email { get; set; } 
        public string Address { get; set; }
        
        public int User_id { get; set; }
        public virtual User User { get; set; } 
    }
}
