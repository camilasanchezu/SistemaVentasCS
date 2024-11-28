using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.DTO;


namespace SistemaVenta.BLL.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> Lista(); // Existing method to list users
        Task<SesionDTO> ValidarCredenciales(string correo, string clave); // Validate credentials
        Task<UsuarioDTO> Crear(UsuarioDTO modelo); // Create user
        Task<bool> Editar(UsuarioDTO modelo); // Edit user
        Task<bool> Eliminar(int id); // Delete user

        // New Methods for Role-Based Validation
        Task<string> ObtenerRolUsuario(int userId); // Fetch the user's role by ID
        Task<bool> ValidarAccesoPorRol(int userId, string roleName); // Check if user has specific role
        Task<bool> ValidarAccesoPorUsuarioId(int userId); // Check if userId is allowed

    }
}
