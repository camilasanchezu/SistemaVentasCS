using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class VentaService: IVentaService
    {
        private readonly IVentaRepository _ventaRepositorio;
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepositorio;
        private readonly IMapper _mapper;

        public VentaService(IVentaRepository ventaRepositorio, IGenericRepository<DetalleVenta> detalleVentaRepositorio, IMapper mapper)
        {
            _ventaRepositorio = ventaRepositorio;
            _detalleVentaRepositorio = detalleVentaRepositorio;
            _mapper = mapper;
        }


        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                var ventaGenerada = await _ventaRepositorio.Registrar(_mapper.Map<Venta>(modelo));  

                if(ventaGenerada.IdVenta ==0)
                    throw new TaskCanceledException("No se pudo registrar la venta");

                return _mapper.Map<VentaDTO>(ventaGenerada);

            }
            catch
            {
                throw;
            }
        }
        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _ventaRepositorio.Consultar();
            var ListaResultado = new List<Venta>();

            try
            {
                if(buscarPor == "fecha")
                {
                    DateTime fech_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-EC"));
                    DateTime fech_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-EC"));

                    ListaResultado = await query.Where
                        (v => v.FechaRegistro.Value.Date >= fech_Inicio.Date && 
                        v.FechaRegistro.Value.Date <= fech_Fin.Date
                        ).Include(dv => dv.DetalleVenta).ThenInclude
                        (p => p.IdProductoNavigation).ToListAsync();
                }
                else
                {
                    ListaResultado = await query.Where
                       (v => v.NumeroDocumento == numeroVenta
                       ).Include(dv => dv.DetalleVenta).ThenInclude
                       (p => p.IdProductoNavigation).ToListAsync();

                }

            }
            catch
            {
                throw;

            }

            return _mapper.Map<List<VentaDTO>>(ListaResultado);
        }
     
        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
        {
            IQueryable<DetalleVenta> query = await _detalleVentaRepositorio.Consultar();
            var ListaResultado = new List<DetalleVenta>();

            try
            {
                DateTime fech_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-EC"));
                DateTime fech_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-EC"));

                ListaResultado = await query.
                    Include(p =>p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= fech_Inicio.Date && 
                    dv.IdVentaNavigation.FechaRegistro.Value.Date <= fech_Fin.Date).ToListAsync();

            }
            catch
            {
                throw;

            }
            return _mapper.Map<List<ReporteDTO>>(ListaResultado);

        }

        public async Task<List<CompararVentasDTO>> CompararVentasEntreMeses(List<MesesDTO> meses)
        {
            try
            {
                // Resolvemos la consulta de ventas
                var ventasQuery = await _ventaRepositorio.Consultar();

                List<CompararVentasDTO> resultadosComparacion = new List<CompararVentasDTO>();

                // Iteramos sobre cada mes recibido
                foreach (var mes in meses)
                {
                    // Crear las fechas de inicio y fin para el mes actual
                    var fechaInicioMesActual = new DateTime(DateTime.Now.Year, mes.Mes, 1); // Primer día del mes
                    var fechaFinMesActual = new DateTime(DateTime.Now.Year, mes.Mes, DateTime.DaysInMonth(DateTime.Now.Year, mes.Mes)); // Último día del mes

                    // Calculamos el mes anterior
                    var mesAnterior = mes.Mes == 1 ? 12 : mes.Mes - 1; // Si es enero (mes 1), el mes anterior es diciembre (mes 12)
                    var fechaInicioMesAnterior = new DateTime(DateTime.Now.Year, mesAnterior, 1);
                    var fechaFinMesAnterior = new DateTime(DateTime.Now.Year, mesAnterior, DateTime.DaysInMonth(DateTime.Now.Year, mesAnterior));

                    // Consultamos las ventas para el mes anterior
                    var totalMesAnterior = await ventasQuery
                        .Where(v => v.FechaRegistro >= fechaInicioMesAnterior && v.FechaRegistro <= fechaFinMesAnterior)
                        .SumAsync(v => v.Total ?? 0); // Sumar el total de ventas del mes anterior

                    // Consultamos las ventas para el mes actual
                    var totalMesActual = await ventasQuery
                        .Where(v => v.FechaRegistro >= fechaInicioMesActual && v.FechaRegistro <= fechaFinMesActual)
                        .SumAsync(v => v.Total ?? 0); // Sumar el total de ventas del mes actual

                    // Guardamos el resultado de los dos meses en la lista de comparación
                    resultadosComparacion.Add(new CompararVentasDTO
                    {
                        TotalMes = totalMesActual,
                        Diferencia = Math.Abs(totalMesActual - totalMesAnterior), // Diferencia absoluta entre los dos meses
                        EstadoComparacion = totalMesActual > totalMesAnterior ? "Aumentaron" : "Disminuyeron" // Determinar el estado de la comparación
                    });
                }

                return resultadosComparacion;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al comparar las ventas entre meses", ex);
            }
        }




    }
}
