using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class CompararVentasDTO
    {
        public decimal TotalMes { get; set; }
        public decimal Diferencia { get; set; }
        public string EstadoComparacion { get; set; }
    }
}
