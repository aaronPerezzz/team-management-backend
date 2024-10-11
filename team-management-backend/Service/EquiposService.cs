using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using team_management_backend.Context;
using team_management_backend.DTOs;
using team_management_backend.Exceptions;
using team_management_backend.Interface;
using team_management_backend.Models;
using team_management_backend.Utils;
using team_management_backend.Utils.Pagination;

/*
    @author Aaron Pérez
    @since 09/10/2024
    Servicio de equipos
*/
namespace team_management_backend.Service
{
    public class EquiposService : IEquipos
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<Usuario> userManager;
        private readonly ITipoEquipo tipoEquipoService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EquiposService(
            ApplicationDbContext context,
            UserManager<Usuario> userManager,
             ITipoEquipo tipoEquipoService,
             IMapper mapper,
             IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.tipoEquipoService = tipoEquipoService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
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
            saveEquipo.Estatus = Constantes.TRUE;
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
        /// Eliminar 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteById(int id, Equipo equipment)
        {
            Usuario userInformation = await UserInformation();
            equipment.Estatus = Constantes.FALSE;
            equipment.FechaModificacion = DateTime.Now;
            equipment.IdUsuarioModificacion = userInformation.Id;
            await context.SaveChangesAsync();

        }

        /// <summary>
        /// Obtiene todos los equipos registrados
        /// </summary>
        /// <returns>List<EquipoDTO></returns>
        public async Task<List<EquipoDTO>> GetEquipmentAll(PaginationDTO pagination)
        {
            IQueryable<Equipo> queryTypeEquipment = context.Equipos
                .Include(g => g.Garantia)
                .Include(p => p.Poliza)
                .Include(ct => ct.CaracteristicasTransporte)
                .Include(s => s.Software)
                .Include(h => h.Hardware)
                .Where(e => e.Estatus)
                .AsQueryable();

            List<Equipo> typeEquipmentPagination = await queryTypeEquipment.Paginar(pagination).ToListAsync();
            return mapper.Map<List<EquipoDTO>>(queryTypeEquipment);
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
                .FirstOrDefaultAsync(e => e.Id == id && e.Estatus);

            return mapper.Map<EquipoDTO>(findById);
        }

        /// <summary>
        /// Regresa listado de equipos por tipo
        /// </summary>
        /// <param name="type"></param>
        /// <returns>EquipoDTO</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<EquipoDTO>> GetFindByType(string type, PaginationDTO pagination)
        {
            IQueryable<Equipo> querybleEquipmentByType = context.Equipos
                .Include(e => e.TipoEquipo)
                .Where(t => t.TipoEquipo.Nombre == type && t.Estatus)
                .Select(s => new Equipo
                {
                    Id = s.Id,
                    IdTipoEquipo = s.IdTipoEquipo,
                    Marca = s.Marca,
                    Modelo = s.Modelo,
                    Estado = s.Estado,
                    Serial = s.Serial,
                    Estatus = s.Estatus,
                    Garantia = s.Garantia,
                    Poliza = s.Poliza,
                    CaracteristicasTransporte = s.CaracteristicasTransporte,
                    Software = s.Software,
                    Hardware = s.Hardware,
                }).AsQueryable();
            List<Equipo> listEquipment = await querybleEquipmentByType.Paginar(pagination).ToListAsync();

            return mapper.Map<List<EquipoDTO>>(listEquipment);
        }

        /// <summary>
        /// Modificacion de información
        /// </summary>
        /// <param name="id"></param>
        /// <param name="equipo"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdateEquipmentById(int id, Equipo equipmentEntity, EditarEquipoDTO equipo)
        {
            if (equipmentEntity.IdTipoEquipo != equipo.IdTipoEquipo)
            {
                throw new CustomException("El tipo de modificación no esta permitido");
            }
            Usuario userInformation = await UserInformation();
            DateTime dateTime = DateTime.Now;
            if (equipo.Estado is not null)
            {
                equipmentEntity.Estado = equipo.Estado;
            }
            if (equipo.Poliza is not null)
            {
                PolizaDTO polizaDTO = equipo.Poliza;
                Poliza polizaEntity = await context.Polizas.FindAsync(equipo.Poliza.Id);
                polizaEntity.Aseguradora = polizaDTO.Aseguradora;
                polizaEntity.Numero_poliza = polizaDTO.Numero_poliza;
                polizaEntity.Cobertura = polizaDTO.Cobertura;
                polizaEntity.FechaInicio = polizaDTO.FechaInicio;
                polizaEntity.FechaFin = polizaDTO.FechaFin;
                polizaEntity.IdUsuarioModificacion = userInformation.Id;
                polizaEntity.FechaModificacion = dateTime;
            }
            TipoEquipo typeEquipmentEntity = await context.TiposEquipo.FindAsync(equipo.IdTipoEquipo);
            if (typeEquipmentEntity.Nombre.Equals(Constantes.TRANSPORTE))
            {

                CaracteristicasTransporteEditarDTO caracteristicas = equipo.CaracteristicasTransporte;
                CaracteristicasTransporte transporteEntity = await context.CaracteristicasTransportes.FindAsync(caracteristicas.Id);
                transporteEntity.Placas = caracteristicas.Placas.ToUpper();
                transporteEntity.Color = caracteristicas.Color.ToUpper();
                transporteEntity.Transmision = caracteristicas.Transmision.ToUpper();
                transporteEntity.IdUsuarioCreacion = userInformation.Id;
                transporteEntity.FechaModificacion = dateTime;

            } else if (typeEquipmentEntity.Nombre.Equals(Constantes.ELECTRONICO))
            {
                List<SoftwareEditarDTO> listSoftware = equipo.Software;
                if (listSoftware is not null && listSoftware.Count > 0)
                {
                    foreach (SoftwareEditarDTO softwareDTO in listSoftware)
                    {
                        Software softawereEntity = new Software();
                        if (softwareDTO.Id != 0)
                        {
                            softawereEntity = await context.Softwares.FindAsync(softwareDTO.Id);
                        }
                        softawereEntity.IdEquipo = id;
                        softawereEntity.Serial = softwareDTO.Serial is null ? softawereEntity.Serial : softwareDTO.Serial;
                        softawereEntity.Version = softwareDTO.Version is null ? softawereEntity.Version : softwareDTO.Version;
                        softawereEntity.FechaCompra = softwareDTO.FechaCompra.ToString() is null ? softawereEntity.FechaCompra : softwareDTO.FechaCompra;
                        softawereEntity.IdUsuarioModificacion = userInformation.Id;
                        softawereEntity.FechaModificacion = dateTime;
                    }
                }
                List<HardwareDTO> listHardware = equipo.Hardware;
                if (listHardware is not null && listHardware.Count > 0)
                {
                    List<Hardware> hardwareList = new List<Hardware>();
                    foreach (HardwareDTO hardwareDTO in listHardware)
                    {
                        Hardware hardware = new Hardware();
                        hardware.Marca = hardwareDTO.Marca;
                        hardware.Nombre = hardwareDTO.Nombre;
                        hardware.Descripcion = hardwareDTO.Descripcion is null ? string.Empty : hardwareDTO.Descripcion;
                        hardware.Serial = hardwareDTO.Serial.ToUpper();
                        hardware.IdUsuarioCreacion = userInformation.Id;
                        hardware.IdUsuarioModificacion = userInformation.Id;
                        hardware.FechaCreacion = dateTime;
                        hardware.FechaModificacion = dateTime;
                        hardwareList.Add(hardware);
                    }
                }
            } else
            {
                throw new CustomException("Error al editar");
            }
            equipmentEntity.IdUsuarioModificacion = userInformation.Id;
            equipmentEntity.FechaModificacion = dateTime;

            int statusUpdate = await context.SaveChangesAsync();
            if (statusUpdate == 0)
            {
                throw new CustomException("Problemas al actualizar la información");
            }
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

        /// <summary>
        /// Busqueda de usuario por correo
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Usuario</returns>
        /// <exception cref="CustomException"></exception>
        private async Task<Usuario> UserInformation()
        {
            string claimEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            Usuario userInformation = await userManager.FindByEmailAsync(claimEmail);
            if (userInformation is null)
            {
                throw new CustomException("Usuario no encontrado.");
            }
            return userInformation;
        }
    }
}
