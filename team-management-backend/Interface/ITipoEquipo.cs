using team_management_backend.DTOs;

namespace team_management_backend.Interface
{
    public interface ITipoEquipo
    {
        Task<List<TipoEquipoDTO>> GetAll();
        Task<TipoEquipoDTO> GetById(int id);
    }
}
