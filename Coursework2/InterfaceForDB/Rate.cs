using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceForDB
{
    public class Rate
    {
        public int Id { get; set; } // Первинний ключ
        public decimal Per_min_rate { get; set; } // Вартість за хвилину
        public decimal Per_km_rate { get; set; } // Вартість за кілометр
        public string type { get; set; } // Назва тарифу
        public decimal Base_fare { get; set; } // Базовий тариф (плата за посадку)
    }
}
