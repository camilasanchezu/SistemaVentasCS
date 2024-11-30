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

        public async Task<ComparacionVentasDTO> CompararHistorialYReporte(string fechaInicio, string fechaFin)
        {
            try
            {
                // Convertir las fechas
                DateTime fech_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-EC"));
                DateTime fech_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-EC"));

                // Obtener ventas del historial y del reporte
                var ventasHistorial = await _ventaRepositorio.Consultar();
                var ventasReporte = await _detalleVentaRepositorio.Consultar();

                // Filtrar las ventas por rango de fechas
                var historialFiltrado = await ventasHistorial
                    .Where(v => v.FechaRegistro >= fech_Inicio && v.FechaRegistro <= fech_Fin)
                    .Include(v => v.DetalleVenta)
                    .ThenInclude(dv => dv.IdProductoNavigation)
                    .ToListAsync();

                var reporteFiltrado = await ventasReporte
                    .Where(dv => dv.IdVentaNavigation.FechaRegistro >= fech_Inicio && dv.IdVentaNavigation.FechaRegistro <= fech_Fin)
                    .Include(dv => dv.IdProductoNavigation)
                    .ToListAsync();

                // Agrupar productos del historial y del reporte
                var historialAgrupado = historialFiltrado
                    .SelectMany(v => v.DetalleVenta)
                    .GroupBy(dv => dv.IdProducto)
                    .Select(g => new
                    {
                        IdProducto = g.Key,
                        NombreProducto = g.First().IdProductoNavigation.Nombre,
                        Cantidad = g.Sum(dv => dv.Cantidad ?? 0)
                    }).ToList();

                var reporteAgrupado = reporteFiltrado
                    .GroupBy(dv => dv.IdProducto)
                    .Select(g => new
                    {
                        IdProducto = g.Key,
                        NombreProducto = g.First().IdProductoNavigation.Nombre,
                        Cantidad = g.Sum(dv => dv.Cantidad ?? 0)
                    }).ToList();

                // Crear lista para las comparaciones
                var comparaciones = new List<ComparacionVentasDTO>();

                // Iterar sobre el historial agrupado con un foreach
                foreach (var productoHistorial in historialAgrupado)
                {
                    // Buscar el producto en el reporte
                    var productoReporte = reporteAgrupado.FirstOrDefault(r => r.IdProducto == productoHistorial.IdProducto);

                    // Calcular los valores necesarios
                    int cantidadReporte = productoReporte?.Cantidad ?? 0;
                    int cantidadTotal = productoHistorial.Cantidad + cantidadReporte;
                    int diferencia = Math.Abs(productoHistorial.Cantidad - cantidadReporte);

                    // Agregar la comparación a la lista
                    comparaciones.Add(new ComparacionVentasDTO
                    {
                        IdProducto = productoHistorial.IdProducto ?? 0, // Si es nulo, asigna 0
                        NombreProducto = productoHistorial.NombreProducto ?? "Producto desconocido",
                        CantidadVendida = cantidadTotal,
                        PorcentajeVenta = Math.Round((double)cantidadTotal / (historialAgrupado.Sum(h => h.Cantidad) + reporteAgrupado.Sum(r => r.Cantidad)) * 100, 2),
                        TipoPago = historialFiltrado.FirstOrDefault(v => v.DetalleVenta.Any(dv => dv.IdProducto == productoHistorial.IdProducto))?.TipoPago ?? "Sin información",
                        NumeroDocumento = historialFiltrado.FirstOrDefault(v => v.DetalleVenta.Any(dv => dv.IdProducto == productoHistorial.IdProducto))?.NumeroDocumento ?? "No especificado"
                    });

                }

                // Determinar el producto más vendido
                var productoMasVendido = comparaciones.OrderByDescending(c => c.CantidadVendida).FirstOrDefault();

                return productoMasVendido ?? throw new Exception("No se encontraron datos para comparar en el rango proporcionado.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al comparar historial y reporte", ex);
            }
        }

        public async Task<List<CategoriaVentasDTO>> ObtenerVentasPorCategoria(string fechaInicio, string fechaFin)
        {
            try
            {
                // Convertir las fechas al formato adecuado
                DateTime fech_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-EC"));
                DateTime fech_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-EC"));

                // Consultar las ventas en el rango de fechas
                var ventasQuery = await _detalleVentaRepositorio.Consultar();

                // Filtrar las ventas por rango de fechas y agruparlas por categoría
                var ventasAgrupadasPorCategoria = await ventasQuery
                    .Include(dv => dv.IdProductoNavigation)
                    .ThenInclude(p => p.IdCategoriaNavigation)
                    .Where(dv => dv.IdVentaNavigation.FechaRegistro >= fech_Inicio && dv.IdVentaNavigation.FechaRegistro <= fech_Fin)
                    .GroupBy(dv => new
                    {
                        dv.IdProductoNavigation.IdCategoria,
                        NombreCategoria = dv.IdProductoNavigation.IdCategoriaNavigation.Nombre
                    })
                    .Select(g => new CategoriaVentasDTO
                    {
                        IdCategoria = g.Key.IdCategoria ?? 0, // Si la categoría es nula, usar 0
                        NombreCategoria = g.Key.NombreCategoria ?? "Categoría desconocida",
                        TotalVentas = g.Sum(dv => dv.Cantidad * dv.Precio ?? 0) // Calcular el total de ventas
                    })
                    .ToListAsync();

                return ventasAgrupadasPorCategoria;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ventas por categoría", ex);
            }
        }






    }
}
