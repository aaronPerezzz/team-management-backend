using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using team_management_backend.Context;
using team_management_backend.domain.Entities;
using team_management_backend.Domain.Interfaces.Repository;
using team_management_backend.Exceptions;
using team_management_backend.Utils;
using team_management_backend.Web.Model;

namespace team_management_backend.Domain.Interfaces.Service
{
    public class TipoEquipoService : ITipoEquipo
    {
   
        private readonly ApplicationDbContext context;

        public TipoEquipoService(
            ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public async Task<List<TipoEquipo>> GetAll()
        {
           return await context.TiposEquipo.ToListAsync();
        }
    }
}
