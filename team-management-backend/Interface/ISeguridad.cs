using team_management_backend.DTOs;



/*
 * @author Aaron Pérez
 * @since 07/10/2024
 */
namespace team_management_backend.Interface
{
    public interface ISeguridad
    {
        Task<string> Login(UsuarioModel usuario);
        Task<List<RolModel>> Roles();
        Task<List<UsuarioModel>> GetAllUsers();
        Task<string> EditRol(UsuarioModel usuario);
    }
}
