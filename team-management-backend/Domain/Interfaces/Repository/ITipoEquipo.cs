using team_management_backend.domain.Entities;

namespace team_management_backend.Domain.Interfaces.Repository
{
    public interface ITipoEquipo
    {
        Task<List<TipoEquipo>> GetAll();
    }
}
