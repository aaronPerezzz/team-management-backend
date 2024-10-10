using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using team_management_backend.Context;
using team_management_backend.domain.Entities;
using team_management_backend.Domain.Interfaces.Repository;
using team_management_backend.Exceptions;
using team_management_backend.Utils;
using team_management_backend.Web.Model;

/*
    @author Aaron Pérez
    @since 09/1072024
    Servicio tipos de equipos
*/
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

        /// <summary>
        /// Obtiene todos los tipos de equipos
        /// </summary>
        /// <returns>List<TipoEquipoDTO></returns>
        public async Task<List<TipoEquipoDTO>> GetAll()
        {
            List<TipoEquipo> typeEquipment = await context.TiposEquipo.ToListAsync();
            return mapper.Map<List<TipoEquipoDTO>>(typeEquipment);
        }

        /// <summary>
        /// Obtiene TipoEquipoDTO por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TipoEquipoDTO</returns>
        public async Task<TipoEquipoDTO> GetById(int id)
        {
            TipoEquipo findId = await context.TiposEquipo.FindAsync(id);
            return mapper.Map<TipoEquipoDTO>(findId);
        }
    }
}
