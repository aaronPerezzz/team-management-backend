using team_management_backend.domain.Entities;
using team_management_backend.Web.Model;

namespace team_management_backend.Domain.Interfaces.Repository
{
    public interface IEquipos
    {
        Task<Equipo> SaveEquipment(Equipo equipo);
        Task<List<Equipo>> GetEquipmentAll();
        Task<Equipo> GetFindById(int id);
        Task<Equipo> GetFindByType(string type);
        Task<Equipo> UpdateEquipmentById(int id, Equipo equipo);
        Task<Equipo> DeleteById(int id);
    }
}
