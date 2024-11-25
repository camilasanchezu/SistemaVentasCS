using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;
using System.Globalization;


namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaServicio;

        public VentaController(IVentaService ventaServicio)
        {
            _ventaServicio = ventaServicio;
        }

        [HttpPost]
        [Route("Registrar")]

        public async Task<IActionResult> Registrar([FromBody] VentaDTO venta)
        {
            var rsp = new Response<VentaDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _ventaServicio.Registrar(venta);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("Historial")]

        public async Task<IActionResult> Historial(string buscarPor, string? numeroVenta, string? fechaInicio, string? fechaFin)
        {
            var rsp = new Response<List<VentaDTO>>();
            numeroVenta = numeroVenta is null ? "" : numeroVenta;
            fechaInicio = fechaInicio is null ? "" : fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;

            try
            {
                rsp.status = true;
                rsp.value = await _ventaServicio.Historial(buscarPor, numeroVenta, fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("Reporte")]

        public async Task<IActionResult> Reporte(string? fechaInicio, string? fechaFin)
        {
            var rsp = new Response<List<ReporteDTO>>();
            

            try
            {
                rsp.status = true;
                rsp.value = await _ventaServicio.Reporte( fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);
        }

        [HttpPost("CompararVentas")]
        public async Task<IActionResult> CompararVentas([FromBody] List<MesesDTO> meses)
        {
            var rsp = new Response<List<CompararVentasDTO>>();

            try
            {
                var resultados = new List<CompararVentasDTO>();

                // Recorremos la lista de meses
                foreach (var mes in meses)
                {
                    // Creamos las fechas de inicio y fin para el mes seleccionado
                    var fechaInicioMesActual = new DateTime(DateTime.Now.Year, mes.Mes, 1); // Primer día del mes
                    var fechaFinMesActual = new DateTime(DateTime.Now.Year, mes.Mes, DateTime.DaysInMonth(DateTime.Now.Year, mes.Mes)); // Último día del mes

                    // Calculamos el mes anterior
                    var mesAnterior = mes.Mes == 1 ? 12 : mes.Mes - 1; // Si es enero (mes 1), el mes anterior es diciembre (mes 12)
                    var fechaInicioMesAnterior = new DateTime(DateTime.Now.Year, mesAnterior, 1);
                    var fechaFinMesAnterior = new DateTime(DateTime.Now.Year, mesAnterior, DateTime.DaysInMonth(DateTime.Now.Year, mesAnterior));

                    // Crea un objeto MesesDTO para el mes actual y el mes anterior
                    var mesesDTO = new List<MesesDTO>
            {
                new MesesDTO { Mes = mes.Mes }, // Mes actual
                new MesesDTO { Mes = mesAnterior } // Mes anterior
            };

                    // Llama al método CompararVentasEntreMeses pasando la lista de MesesDTO
                    var ventas = await _ventaServicio.CompararVentasEntreMeses(mesesDTO);

                    // Agregamos los resultados de la comparación de ventas
                    resultados.AddRange(ventas);
                }

                rsp.status = true;
                rsp.value = resultados;
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp); // Retorna la respuesta con los resultados
        }





    }
}
