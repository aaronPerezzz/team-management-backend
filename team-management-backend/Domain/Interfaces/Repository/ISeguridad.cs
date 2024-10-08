using team_management_backend.Web.Model;


/*
 * @author Aaron Pérez
 * @since 07/10/2024
 */
namespace team_management_backend.domain.Interfaces.Repository
{
    public interface ISeguridad
    {
        Task<string> Login(UsuarioModel usuario);
        Task<List<RolModel>> Roles();
        Task<List<UsuarioModel>> GetAllUsers();
        Task<string> EditRol(UsuarioModel usuario);
    }
}
