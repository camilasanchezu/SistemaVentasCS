﻿using System;
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
    public class ProductoService :IProductoService
    {
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IGenericRepository<Venta> _ventaRepositorio;
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepositorio;
        private readonly IGenericRepository<Categoria> _categoriaRepositorio;
        private readonly IMapper _mapper;

        public ProductoService(
       IGenericRepository<Producto> productoRepositorio,
       IGenericRepository<Venta> ventaRepositorio,
       IGenericRepository<DetalleVenta> detalleVentaRepositorio,
       IGenericRepository<Categoria> categoriaRepositorio,
       IMapper mapper)
        {
            _productoRepositorio = productoRepositorio;
            _ventaRepositorio = ventaRepositorio;
            _detalleVentaRepositorio = detalleVentaRepositorio;
            _categoriaRepositorio = categoriaRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                var queryProducto = await _productoRepositorio.Consultar();

                var listaproductos = queryProducto.Include (cat => cat.IdCategoriaNavigation).ToList();
                return _mapper.Map<List<ProductoDTO>>(listaproductos).ToList();


            }
            catch
            {
                throw;
            }
        }
        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try
            {
                var productoCreado = await _productoRepositorio.Crear(_mapper.Map<Producto>(modelo));

                if(productoCreado.IdProducto == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el producto");
                }

                return _mapper.Map<ProductoDTO>(productoCreado);
            }
            catch
            {
                throw;

            }
        }

        public  async Task<bool> Editar(ProductoDTO modelo)
        {
            try
            {
                var productoModelo = _mapper.Map<Producto>(modelo);
                var productoEncontrado = await _productoRepositorio
                    .Obtener(u => u.IdProducto == productoModelo.IdProducto);

                if(productoEncontrado == null)
                    throw new TaskCanceledException("Producto no existe");

                productoEncontrado.Nombre = productoModelo.Nombre;
                productoEncontrado.IdCategoria = productoModelo.IdCategoria;
                productoEncontrado.Stock = productoModelo.Stock;
                productoEncontrado.Precio = productoModelo.Precio;
                productoEncontrado.EsActivo = productoModelo.EsActivo;

                bool respuesta = await _productoRepositorio.Editar(productoEncontrado);

                if(!respuesta)
                    throw new TaskCanceledException("No se pudo editar el producto");

                return respuesta;

            }
            catch
            {
                throw;

            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var productoEncontrado = await _productoRepositorio.
                    Obtener(p => p.IdProducto == id);

                if (productoEncontrado == null)
                    throw new TaskCanceledException("Producto no existe");

                bool respuesta = await _productoRepositorio.Eliminar(productoEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo eliminar el producto");

                return respuesta;


            }
            catch
            {
                throw;

            }
        }


       


    }
}
