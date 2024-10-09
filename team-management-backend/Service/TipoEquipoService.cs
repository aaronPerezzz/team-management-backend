using AutoMapper;
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
        private readonly IMapper mapper;

        public TipoEquipoService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<TipoEquipoDTO>> GetAll()
        {
            List<TipoEquipo> typeEquipment = await context.TiposEquipo.ToListAsync();
            return mapper.Map<List<TipoEquipoDTO>>(typeEquipment);
        }
    }
}
