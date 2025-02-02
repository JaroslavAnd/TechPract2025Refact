using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceForDB
{
    [Table("AuditLog")]
    public class AuditLog
    {
        public int Id { get; set; }
        public string Event_type { get; set; } 
        public string Table_name { get; set; } 
        public int Record_id { get; set; } 
        public string Details {  get; set; }
        public DateTime Event_time { get; set; } 
    }
}
        