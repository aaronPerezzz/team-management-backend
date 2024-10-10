using team_management_backend.Web.Model;

namespace team_management_backend.Domain.Interfaces.Repository
{
    public interface ITipoEquipo
    {
        Task<List<TipoEquipoDTO>> GetAll();
        Task<TipoEquipoDTO> GetById(int id);
    }
}
