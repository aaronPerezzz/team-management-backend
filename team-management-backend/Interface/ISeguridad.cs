using team_management_backend.Web.Model;


/*
 * @author Aaron Pérez
 * @since 07/10/2024
 */
namespace team_management_backend.domain.Interfaces.Repository
{
    public interface ISeguridad
    {
        Task<string> Login(UsuarioDTO usuario);
        Task<List<RolDTO>> Roles();
        Task<List<UsuarioDTO>> GetAllUsers();
        Task<string> EditRol(UsuarioDTO usuario);
        Task<UsuarioDTO> GetUserById(string id);
    }
}
