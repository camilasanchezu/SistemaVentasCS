using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;
using System.Globalization;
using SistemaVenta.BLL.Servicios;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaServicio;
        private readonly IUsuarioService _usuarioServicio; // Service to fetch user info and roles

        public VentaController(IVentaService ventaServicio, IUsuarioService userService)
        {
            _ventaServicio = ventaServicio;
            _usuarioServicio = userService;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromHeader] int userId, [FromBody] VentaDTO venta)
        {
            var rsp = new Response<VentaDTO>();

            try
            {
                // Validar que el usuario sea Id = 1
                var tieneAcceso = await _usuarioServicio.ValidarAccesoPorUsuarioId(userId);
                if (!tieneAcceso)
                {
                    return Forbid("No tienes permiso para realizar esta acción.");
                }

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


        [HttpPost("CompararVentas")]
        public async Task<IActionResult> CompararVentas([FromHeader] int userId, [FromBody] List<MesesDTO> meses)
        {
            var rsp = new Response<List<CompararVentasDTO>>();

            try
            {
                // Validate role
                var userRole = await _usuarioServicio.ObtenerRolUsuario(userId); // Use 'await' here
                if (userRole != "Administrador")
                {
                    return Forbid("You are not authorized to perform this action.");
                }

                var resultados = new List<CompararVentasDTO>();

                foreach (var mes in meses)
                {
                    var fechaInicioMesActual = new DateTime(DateTime.Now.Year, mes.Mes, 1);
                    var fechaFinMesActual = new DateTime(DateTime.Now.Year, mes.Mes, DateTime.DaysInMonth(DateTime.Now.Year, mes.Mes));

                    var mesAnterior = mes.Mes == 1 ? 12 : mes.Mes - 1;
                    var fechaInicioMesAnterior = new DateTime(DateTime.Now.Year, mesAnterior, 1);
                    var fechaFinMesAnterior = new DateTime(DateTime.Now.Year, mesAnterior, DateTime.DaysInMonth(DateTime.Now.Year, mesAnterior));

                    var mesesDTO = new List<MesesDTO>
            {
                new MesesDTO { Mes = mes.Mes },
                new MesesDTO { Mes = mesAnterior }
            };

                    var ventas = await _ventaServicio.CompararVentasEntreMeses(mesesDTO);
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

            return Ok(rsp);
        }


        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Historial(string buscarPor, string? numeroVenta, string? fechaInicio, string? fechaFin)
        {
            var rsp = new Response<List<VentaDTO>>();
            numeroVenta = numeroVenta ?? "";
            fechaInicio = fechaInicio ?? "";
            fechaFin = fechaFin ?? "";

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
                rsp.value = await _ventaServicio.Reporte(fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }
    }
}
