using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class ComparacionVentasDTO
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadVendida { get; set; }
        public double PorcentajeVenta { get; set; }
        public string TipoPago { get; set; }
        public string NumeroDocumento { get; set; }
    }
}
