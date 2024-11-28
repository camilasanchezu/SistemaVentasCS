using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioServicio;


        public UsuarioController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<UsuarioDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioServicio.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);
        }


        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
        {
            var rsp = new Response<SesionDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioServicio.ValidarCredenciales(login.Correo, login.Clave);
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
        public async Task<IActionResult> Guardar([FromHeader] int userId, [FromBody] UsuarioDTO usuario)
        {
            var rsp = new Response<UsuarioDTO>();

            try
            {
                // Validar permisos: solo el usuario con Id = 1 o un administrador puede guardar
                var tieneAcceso = await _usuarioServicio.ValidarAccesoPorUsuarioId(userId);
                if (!tieneAcceso)
                {
                    return Forbid("No tienes permiso para realizar esta acción.");
                }

                // Validar modelo recibido
                if (usuario == null)
                {
                    rsp.status = false;
                    rsp.msg = "El modelo de usuario es nulo.";
                    return BadRequest(rsp);
                }

                if (string.IsNullOrWhiteSpace(usuario.NombreCompleto) ||
                    string.IsNullOrWhiteSpace(usuario.Correo) ||
                    usuario.IdRol <= 0)
                {
                    rsp.status = false;
                    rsp.msg = "Los datos del usuario son inválidos o incompletos.";
                    return BadRequest(rsp);
                }

                // Intentar guardar el usuario
                rsp.status = true;
                rsp.value = await _usuarioServicio.Crear(usuario);
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
        public async Task<IActionResult> Editar([FromHeader] int userId, [FromBody] UsuarioDTO usuario)
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
                if (usuario == null || usuario.IdUsuario <= 0)
                {
                    rsp.status = false;
                    rsp.msg = "El modelo de usuario es inválido o incompleto.";
                    return BadRequest(rsp);
                }

                // Intentar editar el usuario
                rsp.status = true;
                rsp.value = await _usuarioServicio.Editar(usuario);
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

                // Validar que el ID sea válido
                if (id <= 0)
                {
                    rsp.status = false;
                    rsp.msg = "El ID del usuario es inválido.";
                    return BadRequest(rsp);
                }

                // Intentar eliminar el usuario
                rsp.status = true;
                rsp.value = await _usuarioServicio.Eliminar(id);
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
