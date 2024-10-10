using System.IO.Compression;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using team_management_backend.Context;
using team_management_backend.domain.Entities;
using team_management_backend.Domain.Interfaces.Repository;
using team_management_backend.DTOs;
using team_management_backend.Exceptions;
using team_management_backend.Web.Model;

/*
    @author Aaron Pérez
    @since 09/10/2024
    Servicio de equipos
*/
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

        /// <summary>
        /// Guarda información de equipos
        /// </summary>
        /// <param name="equipo"></param>
        /// <returns>EquipoDTO</returns>
        /// <exception cref="CustomException"></exception>
        public async Task<EquipoDTO> SaveEquipment(EquipoDTO equipo)
        {
            TipoEquipoDTO typesEquitment = await SearchTypeEquipment(equipo.IdTipoEquipo);
            Usuario userInformation = await FindUserByEmail(equipo.CorreoUsuario);
            Equipo saveEquipo = mapper.Map<Equipo>(equipo);
            List<Hardware> listHardware = new List<Hardware>();
            List<Software> listSoftware = new List<Software>();
            DateTime dateNow = DateTime.Now;

            saveEquipo.IdTipoEquipo = typesEquitment.Id;
            saveEquipo.FechaCreacion = dateNow;
            saveEquipo.FechaModificacion = dateNow;
            saveEquipo.IdUsuarioCreacion = userInformation.Id;
            saveEquipo.IdUsuarioModificacion = userInformation.Id;
            SavePolizaAndGarantia(saveEquipo, userInformation, equipo, dateNow);

            if (typesEquitment.Nombre.Equals("Electrónico"))
            {
                if (equipo.Software is null || equipo.Hardware is null)
                {
                    throw new CustomException("Falta información para guardar");
                }
                if (equipo.Hardware.Count > 0)
                {
                    foreach (HardwareDTO h in equipo.Hardware)
                    {
                        Hardware hardware = mapper.Map<Hardware>(h);
                        hardware.IdEquipo = saveEquipo.Id;
                        hardware.IdUsuarioCreacion = userInformation.Id;
                        hardware.IdUsuarioModificacion = userInformation.Id;
                        hardware.FechaCreacion = dateNow;
                        hardware.FechaModificacion = dateNow;
                        listHardware.Add(hardware);
                    }
                    saveEquipo.Hardware = listHardware;
                }
                if (equipo.Software.Count > 0)
                {

                    foreach (SoftwareDTO s in equipo.Software)
                    {
                        Software software = mapper.Map<Software>(s);
                        software.IdEquipo = saveEquipo.Id;
                        software.IdUsuarioCreacion = userInformation.Id;
                        software.IdUsuarioModificacion = userInformation.Id;
                        software.FechaCreacion = dateNow;
                        software.FechaModificacion = dateNow;
                        listSoftware.Add(software);
                    }
                    saveEquipo.Software = listSoftware;
                }


            }
            else if (typesEquitment.Nombre.Equals("Transporte"))
            {
                if (equipo.CaracteristicasTransporte is null)
                {
                    throw new CustomException("Falta información para guardar");
                }

                CaracteristicasTransporte caracteristicasTransporte = mapper.Map<CaracteristicasTransporte>(equipo.CaracteristicasTransporte);
                caracteristicasTransporte.IdEquipo = saveEquipo.Id;
                caracteristicasTransporte.IdUsuarioCreacion = userInformation.Id;
                caracteristicasTransporte.IdUsuarioModificacion = userInformation.Id;
                caracteristicasTransporte.FechaCreacion = dateNow;
                caracteristicasTransporte.FechaModificacion = dateNow;
                saveEquipo.CaracteristicasTransporte = caracteristicasTransporte;
            }
            else
            {
                throw new CustomException("No existe el tipo de equipo seleccionado.");
            }

            context.Add(saveEquipo);
            var resultSave = await context.SaveChangesAsync();

            return mapper.Map<EquipoDTO>(saveEquipo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task IEquipos.DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene todos los equipos registrados
        /// </summary>
        /// <returns>List<EquipoDTO></returns>
        public async Task<List<EquipoDTO>> GetEquipmentAll()
        {
            List<Equipo> listTypeEquipment = await context.Equipos
                .Include(g => g.Garantia)
                .Include(p => p.Poliza)
                .Include(ct => ct.CaracteristicasTransporte)
                .Include(s => s.Software)
                .Include(h => h.Hardware).ToListAsync();
            return mapper.Map<List<EquipoDTO>>(listTypeEquipment);
        }

        /// <summary>
        /// Busca EquipoDTO por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EquipoDTO</returns>
        public async Task<EquipoDTO> GetFindById(int id)
        {
            Equipo findById = await context.Equipos
                .Include(g => g.Garantia)
                .Include(p => p.Poliza)
                 .Include(ct => ct.CaracteristicasTransporte)
                .Include(s => s.Software)
                .Include(h => h.Hardware)
                .FirstOrDefaultAsync(e => e.Id == id);

            return mapper.Map<EquipoDTO>(findById);
        }

        /// <summary>
        /// Regresa listado de equipos por tipo
        /// </summary>
        /// <param name="type"></param>
        /// <returns>EquipoDTO</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<PorTipoEquipoDTO>> GetFindByType(string type)
        {
            List<TipoEquipo> equipmentByType = await context.TiposEquipo
                .Include(e => e.Equipos)
                .ThenInclude(g => g.Garantia)
                .Include(e => e.Equipos)
                .ThenInclude(p => p.Poliza)
                .Include(e => e.Equipos)
                .ThenInclude(c => c.CaracteristicasTransporte)
                .Include(e => e.Equipos)
                .ThenInclude(s => s.Software)
                .Include(e => e.Equipos)
                .ThenInclude(h => h.Hardware)
                .Where(s => s.Nombre == type).ToListAsync();

            return mapper.Map<List<PorTipoEquipoDTO>>(equipmentByType);
        }

        Task IEquipos.UpdateEquipmentById(int id, EquipoDTO equipo)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Busca TipoEquipoDTO por id y valida la información
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TipoEquipoDTO</returns>
        /// <exception cref="CustomException"></exception>
        private async Task<TipoEquipoDTO> SearchTypeEquipment(int id)
        {
            TipoEquipoDTO typesEquitment = await tipoEquipoService.GetById(id);
            if (typesEquitment is null)
            {
                throw new CustomException("No existe el tipo de equipo.");
            }
            return typesEquitment;
        }


        /// <summary>
        /// Busca información de Usuario por email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Usuario</returns>
        /// <exception cref="CustomException"></exception>
        private async Task<Usuario> FindUserByEmail(string email)
        {
            Usuario usuarioId = await userManager.FindByEmailAsync(email);
            if (usuarioId is null)
            {
                throw new CustomException("No existe el usuario.");
            }
            return usuarioId;
        }

        /// <summary>
        /// Guarda información de Poliza y Garantia
        /// </summary>
        /// <param name="saveEquipo"></param>
        /// <param name="userInformation"></param>
        /// <param name="equipo"></param>
        /// <param name="dateNow"></param>
        private void SavePolizaAndGarantia(Equipo saveEquipo, Usuario userInformation, EquipoDTO equipo, DateTime dateNow)
        {
            if (equipo.Poliza is not null)
            {
                Poliza polizaEntity = mapper.Map<Poliza>(equipo.Poliza);
                polizaEntity.IdEquipo = saveEquipo.Id;
                polizaEntity.IdUsuarioCreacion = userInformation.Id;
                polizaEntity.IdUsuarioModificacion = userInformation.Id;
                polizaEntity.FechaCreacion = dateNow;
                polizaEntity.FechaModificacion = dateNow;
                saveEquipo.Poliza = polizaEntity;
            }

            if (equipo.Garantia is not null)
            {
                Garantia garantiaEntity = mapper.Map<Garantia>(equipo.Garantia);
                garantiaEntity.IdEquipo = saveEquipo.Id;
                garantiaEntity.IdUsuarioCreacion = userInformation.Id;
                garantiaEntity.IdUsuarioModificacion = userInformation.Id;
                garantiaEntity.FechaCreacion = dateNow;
                garantiaEntity.FechaModificacion = dateNow;
                saveEquipo.Garantia = garantiaEntity;
            }
        }

        /// <summary>
        /// Busca TipoEquipo por nombre de tipo
        /// </summary>
        /// <param name="type"></param>
        /// <returns>TipoEquipo</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TipoEquipo> GetByTipoEquipo(string type)
        {
            return await context.TiposEquipo.Where(t => t.Nombre == type).FirstOrDefaultAsync();
        }
    }
}
