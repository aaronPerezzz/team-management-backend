using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using team_management_backend.Context;
using team_management_backend.Models;
using team_management_backend.Exceptions;
using team_management_backend.Interface;
using team_management_backend.DTOs.Asignaciones;
using team_management_backend.Utils;
using team_management_backend.Utils.Pagination;

/**
 * @author Alejandro Martínez
 * @since 08/10/2024
 */
namespace team_management_backend.Service
{
    public class AsignacionService : IAsignacion
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<Usuario> userManager;
        private readonly IMapper mapper;

        public AsignacionService(ApplicationDbContext context, UserManager<Usuario> userManager, IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Obtiene un listado de todas las asignaciones paginadas
        /// </summary>
        /// <param name="pag"></param>
        /// <returns>List<AsignacionRegistroDTO></returns>
        public async Task<List<AsignacionRegistroDTO>> GetAllAssignments(PaginationDTO pag)
        {
            var asignacionesQuery = context.Asignaciones
                .Include(a => a.Equipo)
                .Join(context.Users,
                      a => a.IdUsuario,
                      u => u.Id,
                      (a, u) => new
                      {
                          Asignacion = a,
                          u.UserName
                      })
                .Select(x => new AsignacionRegistroDTO
                {
                    Id = x.Asignacion.Id,
                    NombreUsuario = x.UserName, 
                    Marca = x.Asignacion.Equipo.Marca,
                    Modelo = x.Asignacion.Equipo.Modelo,
                    TipoEquipo = x.Asignacion.Equipo.TipoEquipo.Nombre,
                    FechaAsignacion = x.Asignacion.esTemporal ? x.Asignacion.FechaAsignacion : null,
                    FechaFinAsignacion = x.Asignacion.esTemporal ? x.Asignacion.FechaFinAsignacion : null,
                    esTemporal = x.Asignacion.esTemporal
                }).AsQueryable();

            var asignacionesPaginadas = await asignacionesQuery.Paginar(pag).ToListAsync();

            if (asignacionesPaginadas == null || asignacionesPaginadas.Count == 0)
            {
                throw new Exception(Constantes.ERROR_AS01);
            }

            return asignacionesPaginadas;
        }

        /// <summary>
        /// Obtiene un listado de asignaciones 
        /// filtarada por el tipo de equipo
        /// </summary>
        /// <param string tipoEquipo></param>
        /// <param name="pag"></param>
        /// <returns>List<AsignacionRegistroDTO></returns>
        public async Task<List<AsignacionRegistroDTO>> GetAssignmentsByType(string nombreTipoEquipo, PaginationDTO pag)
        {
            var asignacionesQuery = context.Asignaciones
                .Include(a => a.Equipo)
                .ThenInclude(e => e.TipoEquipo)
                .Where(a => a.Equipo.TipoEquipo.Nombre == nombreTipoEquipo)
                .Join(context.Users,
                    a => a.IdUsuario,
                    u => u.Id,
                    (a, u) => new
                    {
                        Asignacion = a,
                        u.UserName
                    })
                .Select(x => new AsignacionRegistroDTO
                {
                    Id = x.Asignacion.Id,
                    NombreUsuario = x.UserName, 
                    Marca = x.Asignacion.Equipo.Marca,
                    Modelo = x.Asignacion.Equipo.Modelo,
                    TipoEquipo = x.Asignacion.Equipo.TipoEquipo.Nombre,
                    FechaAsignacion = x.Asignacion.esTemporal ? x.Asignacion.FechaAsignacion : null,
                    FechaFinAsignacion = x.Asignacion.esTemporal ? x.Asignacion.FechaFinAsignacion : null,
                    esTemporal = x.Asignacion.esTemporal
                })
                .AsQueryable();

            var asignacionesPaginadas = await asignacionesQuery.Paginar(pag).ToListAsync();

            if (asignacionesPaginadas == null || asignacionesPaginadas.Count == 0)
            {
                throw new Exception(Constantes.ERROR_AS01);
            }

            return asignacionesPaginadas;
        }


        /// <summary>
        /// Verifica si el tipo de equipo existe en la base de datos
        /// </summary>
        /// <param string nombreTipoEquipo></param>
        /// <returns>true/false</returns>
        public async Task<bool> ThereIsEquipment(string nombreTipoEquipo)
        {
            return await context.TiposEquipo.AnyAsync(te => te.Nombre == nombreTipoEquipo);
        }

        /// <summary>
        /// Obtiene un listado de asignaciones 
        /// de un usuario
        /// </summary>
        /// <param string correo></param>
        /// <returns>List<AsignacionRegistroDTO></returns>
        public async Task<(List<AsignacionRegistroDTO> lstAsignaciones, string msj)> UserAssignment(string correo)
        {
            var usuario = await userManager.FindByEmailAsync(correo);
            if (usuario == null)
                throw new CustomException("Usuario no encontrado");

            var asignaciones = await context.Asignaciones
                .Include(a => a.Equipo)
                    .ThenInclude(e => e.TipoEquipo)
                .Where(a => a.IdUsuario == usuario.Id)
                .Select(a => new AsignacionRegistroDTO
                {
                    Id = a.Id,
                    NombreUsuario = usuario.UserName,
                    Marca = a.Equipo.Marca,
                    Modelo = a.Equipo.Modelo,
                    TipoEquipo = a.Equipo.TipoEquipo.Nombre,
                    FechaAsignacion = a.esTemporal ? a.FechaAsignacion : null,
                    FechaFinAsignacion = a.esTemporal ? a.FechaFinAsignacion : null,
                    esTemporal = a.esTemporal
                }).ToListAsync();

            // Verificar si hay asignaciones
            if (asignaciones == null)
                throw new CustomException("No existen asignaciones para este usuario");

            return (asignaciones, "Asignaciones obtenidas con éxito");
        }


        /// <summary>
        /// Crea una nueva asignación
        /// </summary>
        /// <param name="asignacion"></param>
        /// <returns>List<AsignacionCrearDTO></returns>
        public async Task<(int id, string msj)> CreateAssignment(AsignacionCrearDTO asignacion)
        {
            var usuario = await userManager.FindByEmailAsync(asignacion.CorreoUsuario);
            var admin = await userManager.FindByEmailAsync(asignacion.CorreoAdministrador);
            if (usuario == null || admin == null) throw new CustomException("Usuario no encontrado");

            var asignacionExistente = await context.Asignaciones
                .FirstOrDefaultAsync(a => a.IdEquipo == asignacion.IdEquipo && a.FechaFinAsignacion == null);
            if (asignacionExistente != null)
            {
                throw new CustomException("El equipo ya está asignado a otro usuario. No puede asignarse nuevamente hasta que se libere.");
            }

            var NuevaAsignacion = mapper.Map<Asignacion>(asignacion);
            NuevaAsignacion.IdUsuario = usuario.Id;
            NuevaAsignacion.IdUsuarioCreacion = admin.Id;
            NuevaAsignacion.FechaCreacion = DateTime.Now; 

            if (asignacion.esTemporal && asignacion.FechaFinAsignacion < asignacion.FechaAsignacion)
            {
                throw new CustomException("La fecha de fin de asignación no puede ser anterior a la fecha de asignación.");
            }

            context.Add(NuevaAsignacion);

            try
            {
                await context.SaveChangesAsync();
                return (NuevaAsignacion.Id, "Asignación creada con éxito");
            }
            catch (Exception ex)
            {
                throw new CustomException("Error al crear la asignación: " + ex.Message);
            }
        }


        /// <summary>
        /// Modifica una asignación, 
        ///puede modifcar el equipo, si es temporal y las fechas de la asignación
        /// </summary>
        /// <param name="asignacion"></param>
        /// <returns>List<AsignacionRegistroDTO></returns>
        public async Task<(int id, string msj)> UpdateAssignment(AsignacionEditarDTO asignacion)
        {
            var usuario = await userManager.FindByEmailAsync(asignacion.CorreoAdministrador);
            if (usuario == null) throw new CustomException(Constantes.ERROR_AS02);

            var asignacionBD = await context.Asignaciones.FirstOrDefaultAsync(x => x.Id == asignacion.Id);
            if (asignacionBD == null) throw new CustomException(Constantes.ERROR_AS03);

            var asignacionExistente = await context.Asignaciones
                .FirstOrDefaultAsync(a => a.IdEquipo == asignacion.IdEquipo && a.Id != asignacion.Id && a.FechaFinAsignacion == null);
            if (asignacionExistente != null)
                {
                throw new CustomException(Constantes.ERROR_AS04);
            }

            asignacionBD = mapper.Map(asignacion, asignacionBD);

            asignacionBD.FechaModificacion = DateTime.Now;
            asignacionBD.IdUsuarioModificacion = usuario.Id;

            var res = await context.SaveChangesAsync();
            if (res == 0) throw new CustomException(Constantes.ERROR_AS05);
            return (asignacionBD.Id, Constantes.MSJ_AS01);
        }


        /// <summary>
        /// Elimina una asignación
        /// </summary>
        /// <param int id></param>
        /// <returns>string</returns>
        public async Task<string> DeleteAssignment(int id)
        {
            var asignacion = await context.Asignaciones.FirstOrDefaultAsync(x => x.Id == id);
            if (asignacion == null) throw new CustomException(Constantes.ERROR_AS03);
            context.Remove(asignacion);
            var respuesta = await context.SaveChangesAsync();
            if (respuesta == 0) throw new CustomException(Constantes.ERROR_AS06);
            return Constantes.MSJ_AS02;
        }

    }
}
