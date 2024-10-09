using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using team_management_backend.Context;
using team_management_backend.domain.Entities;
using team_management_backend.Domain.Interfaces.Repository;
using team_management_backend.Exceptions;
using team_management_backend.Web.Model.Asignaciones;

namespace team_management_backend.Domain.Interfaces.Service
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

        public async Task<List<AsignacionRegistroDTO>> GetAllAssignments()
        {

            var asignaciones = await context.Asignaciones
                .Include(a => a.Equipo)
                .Join(context.Users,
                      a => a.IdUsuario,
                      u => u.Id,
                      (a, u) => new
                      {
                          Asignacion = a,
                          UserName = u.UserName
                      })
                .Select(x => new AsignacionRegistroDTO
                {
                    Id = x.Asignacion.Id,
                    NombreUsuario = x.UserName, 
                    Marca = x.Asignacion.Equipo.Marca,
                    Modelo = x.Asignacion.Equipo.Modelo,
                    TipoEquipo = x.Asignacion.Equipo.TipoEquipo.Nombre,
                    FechaAsignacion = x.Asignacion.esTemporal ? x.Asignacion.FechaAsignacion : (DateTime?)null,
                    FechaFinAsignacion = x.Asignacion.esTemporal ? x.Asignacion.FechaFinAsignacion : (DateTime?)null,
                    esTemporal = x.Asignacion.esTemporal
                })
                .ToListAsync();





            if (asignaciones == null || asignaciones.Count == 0)
            {
                throw new Exception("No se encontraron asignaciones.");
            }

            return asignaciones;
        }


        public async Task<List<AsignacionRegistroDTO>> GetAssignmentsByType(string nombreTipoEquipo)
        {
            var asignaciones = await context.Asignaciones
                .Include(a => a.Equipo)
                .ThenInclude(e => e.TipoEquipo)
                .Where(a => a.Equipo.TipoEquipo.Nombre == nombreTipoEquipo)
                .Join(context.Users,
                    a => a.IdUsuario,
                    u => u.Id,
                    (a, u) => new
                    {
                        Asignacion = a,
                        UserName = u.UserName
                    })
                .Select(x => new AsignacionRegistroDTO
                {
                    Id = x.Asignacion.Id,
                    NombreUsuario = x.UserName, 
                    Marca = x.Asignacion.Equipo.Marca,
                    Modelo = x.Asignacion.Equipo.Modelo,
                    TipoEquipo = x.Asignacion.Equipo.TipoEquipo.Nombre,
                    FechaAsignacion = x.Asignacion.esTemporal ? x.Asignacion.FechaAsignacion : (DateTime?)null,
                    FechaFinAsignacion = x.Asignacion.esTemporal ? x.Asignacion.FechaFinAsignacion : (DateTime?)null,
                    esTemporal = x.Asignacion.esTemporal
                })
                .ToListAsync();


            return asignaciones;
        }

        public async Task<bool> ExisteTipoEquipo(string nombreTipoEquipo)
        {
            return await context.TiposEquipo.AnyAsync(te => te.Nombre == nombreTipoEquipo);
        }

        public async Task<(int id, string msj)> CreateAssignment(AsignacionCrearDTO asignacion)
        {
            var usuario = await userManager.FindByEmailAsync(asignacion.CorreoUsuario);
            if (usuario == null) throw new CustomException("Usuario no encontrado");

            var NuevaAsignacion = mapper.Map<Asignacion>(asignacion);
            NuevaAsignacion.IdUsuario = usuario.Id;
            NuevaAsignacion.IdUsuarioCreacion = usuario.Id;
            NuevaAsignacion.FechaCreacion = DateTime.Now; 

            if (asignacion.esTemporal &&
                (asignacion.FechaFinAsignacion < asignacion.FechaAsignacion))
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

        public async Task<(List<AsignacionRegistroDTO> lstAsignaciones, string msj)> UserAssignment(string correo)
        {
            // Buscar el usuario por correo
            var usuario = await userManager.FindByEmailAsync(correo);
            if (usuario == null)
                throw new CustomException("Usuario no encontrado");

            // Obtener las asignaciones del usuario
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
                    FechaAsignacion = a.esTemporal ? a.FechaAsignacion : (DateTime?)null,
                    FechaFinAsignacion = a.esTemporal ? a.FechaFinAsignacion : (DateTime?)null,
                    esTemporal = a.esTemporal
                }).ToListAsync();

            // Verificar si hay asignaciones
            if (asignaciones == null)
                throw new CustomException("No existen asignaciones para este usuario");

            return (asignaciones, "Asignaciones obtenidas con éxito");
        }


    }
}
