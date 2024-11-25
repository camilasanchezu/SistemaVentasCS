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

        public async Task<List<CompararVentasDTO>> CompararVentasEntreMeses(List<(DateTime fechaInicio, DateTime fechaFin)> meses)
        {
            try
            {
                // Resolve the queryable Venta data first
                var ventasQuery = await _ventaRepositorio.Consultar();

                List<CompararVentasDTO> resultadosComparacion = new List<CompararVentasDTO>();

                // Iterate over each pair of month dates
                foreach (var mes in meses)
                {
                    // Query sales totals for the current month based on FechaRegistro
                    var totalMes = await ventasQuery
                        .Where(v => v.FechaRegistro >= mes.fechaInicio && v.FechaRegistro <= mes.fechaFin)
                        .SumAsync(v => v.Total ?? 0);

                    // Add the result to the list
                    resultadosComparacion.Add(new CompararVentasDTO
                    {
                        TotalMes = totalMes,
                        Diferencia = 0, // No difference to compare yet
                        EstadoComparacion = "N/A" // No comparison yet
                    });
                }

                // Now, compare the totals for each consecutive month pair
                for (int i = 0; i < resultadosComparacion.Count - 1; i++)
                {
                    var mesActual = resultadosComparacion[i];
                    var mesSiguiente = resultadosComparacion[i + 1];

                    decimal diferencia = mesSiguiente.TotalMes - mesActual.TotalMes;
                    mesSiguiente.Diferencia = Math.Abs(diferencia); // Absolute value for difference
                    mesSiguiente.EstadoComparacion = diferencia > 0 ? "Aumentaron" : "Disminuyeron";
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
