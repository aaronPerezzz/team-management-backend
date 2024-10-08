using Microsoft.EntityFrameworkCore;
using team_management_backend.Context;
using team_management_backend.domain.Entities;
using team_management_backend.Domain.Interfaces.Repository;
using team_management_backend.Exceptions;

namespace team_management_backend.Domain.Interfaces.Service
{
    public class EquiposService : IEquipos
    {
        private readonly ApplicationDbContext context;

        public EquiposService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task<Equipo> SaveEquipment(Equipo equipo)
        {
            return null;
        }

        Task<Equipo> IEquipos.DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Equipo>> GetEquipmentAll()
        {
            return await context.Equipos.ToListAsync();
        }

        Task<Equipo> IEquipos.GetFindById(int id)
        {
            throw new NotImplementedException();
        }

        Task<Equipo> IEquipos.GetFindByType(string type)
        {
            throw new NotImplementedException();
        }

        Task<Equipo> IEquipos.UpdateEquipmentById(int id, Equipo equipo)
        {
            throw new NotImplementedException();
        }
    }
}
