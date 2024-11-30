using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class CategoriaVentasDTO
    {
        public int IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
        public decimal TotalVentas { get; set; }
    }
}
