using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using team_management_backend.Context;
using team_management_backend.domain.Entities;
using team_management_backend.Domain.Interfaces.Repository;
using team_management_backend.Exceptions;
using team_management_backend.Web.Model;

namespace team_management_backend.Domain.Interfaces.Service
{
    public class EquiposService : IEquipos
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<Usuario> userManager;
        private readonly ITipoEquipo tipoEquipoService;
        private readonly IMapper mapper;

        public EquiposService(
            ApplicationDbContext context,
            UserManager<Usuario> userManager,
             ITipoEquipo tipoEquipoService,
             IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.tipoEquipoService = tipoEquipoService;
            this.mapper = mapper;
        }

        public async Task<Equipo> SaveEquipment(EquipoDTO equipo)
        {
            List<TipoEquipoDTO> typesEquitment = await tipoEquipoService.GetAll();
            TipoEquipoDTO isCorrectType = typesEquitment.Find(t => t.Id.Equals(equipo.IdTipoEquipo));
            if (isCorrectType is null)
            {
                throw new CustomException("No existe el tipo de equipo");
            }
            DateTime dateNow = DateTime.Now;
            var usuarioId = await userManager.FindByEmailAsync(equipo.CorreoUsuario);
            if (usuarioId is null)
            {
                throw new CustomException("No existe el usuario");
            }

            Equipo saveEquipo = mapper.Map<Equipo>(equipo);
            saveEquipo.FechaCreacion = DateTime.Today;
            saveEquipo.FechaModificacion = DateTime.Today;
            saveEquipo.IdUsuarioCreacion = usuarioId.Id;
            saveEquipo.IdUsuarioModificacion = usuarioId.Id;
            context.Add(saveEquipo);


            if (equipo.Poliza is not null)
            {
                Poliza polizaEntity = mapper.Map<Poliza>(equipo.Poliza);
                polizaEntity.IdEquipo = saveEquipo.Id;
                polizaEntity.IdUsuarioCreacion = usuarioId.Id;
                polizaEntity.IdUsuarioModificacion = usuarioId.Id;
                polizaEntity.FechaCreacion = dateNow;
                polizaEntity.FechaModificacion = dateNow;
                saveEquipo.Poliza = polizaEntity;
                context.Add(polizaEntity);
            }

            if (equipo.Garantia is not null)
            {
                Garantia garantiaEntity = mapper.Map<Garantia>(equipo.Garantia);
                garantiaEntity.IdEquipo = saveEquipo.Id;
                garantiaEntity.IdUsuarioCreacion = usuarioId.Id;
                garantiaEntity.IdUsuarioModificacion = usuarioId.Id;
                garantiaEntity.FechaCreacion = dateNow;
                garantiaEntity.FechaModificacion = dateNow;
                saveEquipo.Garantia = garantiaEntity;
                context.Add(garantiaEntity);
            }

            if (equipo.CaracteristicasTransporte is not null)
            {
                CaracteristicasTransporte caracteristicasTransporte = mapper.Map<CaracteristicasTransporte>(equipo.CaracteristicasTransporte);
                caracteristicasTransporte.IdEquipo = saveEquipo.Id;
                caracteristicasTransporte.IdUsuarioCreacion = usuarioId.Id;
                caracteristicasTransporte.IdUsuarioModificacion = usuarioId.Id;
                caracteristicasTransporte.FechaCreacion = dateNow;
                caracteristicasTransporte.FechaModificacion = dateNow;
                saveEquipo.CaracteristicasTransporte = caracteristicasTransporte;
                context.Add(caracteristicasTransporte);
            }
            var resultSave = await context.SaveChangesAsync();

            return saveEquipo;
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
