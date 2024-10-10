using team_management_backend.domain.Entities;
using team_management_backend.DTOs;
using team_management_backend.Web.Model;

namespace team_management_backend.Domain.Interfaces.Repository
{
    public interface IEquipos
    {
        Task<EquipoDTO> SaveEquipment(EquipoDTO equipo);
        Task<List<EquipoDTO>> GetEquipmentAll();
        Task<EquipoDTO> GetFindById(int id);
        Task<List<PorTipoEquipoDTO>> GetFindByType(string type);
        Task<TipoEquipo> GetByTipoEquipo(string type);
        Task UpdateEquipmentById(int id, EquipoDTO equipo);
        Task DeleteById(int id);
    }
}
