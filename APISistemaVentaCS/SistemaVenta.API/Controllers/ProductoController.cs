using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoServicio;
        private readonly IUsuarioService _usuarioServicio; // Service to fetch user info and roles

        public ProductoController(IProductoService productoServicio, IUsuarioService userService)
        {
            _productoServicio = productoServicio;
            _usuarioServicio = userService;
        }


        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<ProductoDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _productoServicio.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);
        }


        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromHeader] int userId, [FromBody] ProductoDTO producto)
        {
            var rsp = new Response<ProductoDTO>();

            try
            {
                // Validar permisos
                var tieneAcceso = await _usuarioServicio.ValidarAccesoPorUsuarioId(userId);
                if (!tieneAcceso)
                {
                    return Forbid("No tienes permiso para realizar esta acción.");
                }

                // Validar modelo recibido
                if (producto == null)
                {
                    rsp.status = false;
                    rsp.msg = "El modelo del producto no puede ser nulo.";
                    return BadRequest(rsp);
                }

                // Intentar guardar el producto
                rsp.status = true;
                rsp.value = await _productoServicio.Crear(producto);
            }
            catch (TaskCanceledException ex)
            {
                rsp.status = false;
                rsp.msg = $"Error: {ex.Message}";
                return BadRequest(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"Ocurrió un error inesperado: {ex.Message}";
                return StatusCode(500, rsp);
            }

            return Ok(rsp);
        }



        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromHeader] int userId, [FromBody] ProductoDTO producto)
        {
            var rsp = new Response<bool>();

            try
            {
                // Validar permisos
                var tieneAcceso = await _usuarioServicio.ValidarAccesoPorUsuarioId(userId);
                if (!tieneAcceso)
                {
                    return Forbid("No tienes permiso para realizar esta acción.");
                }

                // Validar modelo recibido
                if (producto == null || producto.IdProducto <= 0)
                {
                    rsp.status = false;
                    rsp.msg = "El modelo del producto es inválido.";
                    return BadRequest(rsp);
                }

                // Intentar editar el producto
                rsp.status = true;
                rsp.value = await _productoServicio.Editar(producto);
            }
            catch (TaskCanceledException ex)
            {
                rsp.status = false;
                rsp.msg = $"Error: {ex.Message}";
                return BadRequest(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"Ocurrió un error inesperado: {ex.Message}";
                return StatusCode(500, rsp);
            }

            return Ok(rsp);
        }




        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar([FromHeader] int userId, int id)
        {
            var rsp = new Response<bool>();

            try
            {
                // Validar permisos
                var tieneAcceso = await _usuarioServicio.ValidarAccesoPorUsuarioId(userId);
                if (!tieneAcceso)
                {
                    return Forbid("No tienes permiso para realizar esta acción.");
                }

                // Validar el ID del producto
                if (id <= 0)
                {
                    rsp.status = false;
                    rsp.msg = "El ID del producto es inválido.";
                    return BadRequest(rsp);
                }

                // Intentar eliminar el producto
                rsp.status = true;
                rsp.value = await _productoServicio.Eliminar(id);
            }
            catch (TaskCanceledException ex)
            {
                rsp.status = false;
                rsp.msg = $"Error: {ex.Message}";
                return BadRequest(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"Ocurrió un error inesperado: {ex.Message}";
                return StatusCode(500, rsp);
            }

            return Ok(rsp);
        }




    }
}
