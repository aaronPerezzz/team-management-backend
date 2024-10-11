using team_management_backend.DTOs;
using team_management_backend.Models;
using team_management_backend.Utils.Pagination;

namespace team_management_backend.Interface
{
    public interface IEquipos
    {
        Task<EquipoDTO> SaveEquipment(EquipoDTO equipo);
        Task<List<EquipoDTO>> GetEquipmentAll(PaginationDTO pagination);
        Task<EquipoDTO> GetFindById(int id);
        Task<List<EquipoDTO>> GetFindByType(string type, PaginationDTO pagination);
        Task<TipoEquipo> GetByTipoEquipo(string type);
        Task UpdateEquipmentById(int id, Equipo equipmentEntity, EditarEquipoDTO equipo);
        Task DeleteById(int id, Equipo equipment);
    }
}
