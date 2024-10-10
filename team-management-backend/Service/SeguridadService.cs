﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using team_management_backend.Context;
using team_management_backend.Models;
using team_management_backend.Exceptions;
using team_management_backend.Interface;
using team_management_backend.DTOs;
using team_management_backend.Utils;


/*
 * @author Aaron Pérez
 * @since 07/10/2024
 */
namespace team_management_backend.Service
{
    public class SeguridadService : ISeguridad
    {
        private readonly UserManager<Usuario> userManager;
        private readonly ApplicationDbContext context;
        private readonly JwtService jwtService;

        public SeguridadService(
            UserManager<Usuario> userManager,
            ApplicationDbContext context,
            JwtService jwtService)
        {
            this.userManager = userManager;
            this.context = context;
            this.jwtService = jwtService;
        }

        /// <summary>
        /// Modifica rol de un usuario
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns>string</returns>
        /// <exception cref="CustomException"></exception>
        async Task<string> ISeguridad.EditRol(UsuarioDTO userModel)
        {
            var queryUser = await userManager.FindByEmailAsync(userModel.Correo);
            if (queryUser is null)
            {
                throw new CustomException(Constantes.ERROR_SEG01);
            }

            var previousRole = await userManager.GetRolesAsync(queryUser);
            if (previousRole is not null)
            {
                //Remover el rol anterior
                foreach (string rolesActuales in previousRole)
                {
                    await userManager.RemoveFromRoleAsync(queryUser, rolesActuales);
                }
            }

            var newRol = await userManager.AddToRoleAsync(queryUser, userModel.Rol);
            if (!newRol.Succeeded)
            {
                throw new CustomException(Constantes.ERROR_SEG02);
            }
            return Constantes.MSJ_SEG02;
        }

        /// <summary>
        /// Obtiene todos los usuario
        /// </summary>
        /// <returns>List<UsuarioModel></returns>
        /// <exception cref="CustomException"></exception>
        async Task<List<UsuarioDTO>> ISeguridad.GetAllUsers()
        {
            List<UsuarioDTO> users = new List<UsuarioDTO>();
            try
            {
                users = await (from user in context.Users
                               join userRole in context.UserRoles on user.Id equals userRole.UserId
                               join role in context.Roles on userRole.RoleId equals role.Id
                               select new UsuarioDTO
                               {
                                   NombreCompleto = user.NombreCompleto,
                                   Correo = user.Email,
                                   Rol = role.Name,
                               }).ToListAsync();
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }

            return users;
        }

        async Task<UsuarioDTO> ISeguridad.GetUserById(string email)
        {
            UsuarioDTO userModel;
            try
            {
                userModel = await (from user in context.Users
                                   join userRole in context.UserRoles on user.Id equals userRole.UserId
                                   join role in context.Roles on userRole.RoleId equals role.Id
                                   where user.Email == email
                                   select new UsuarioDTO
                                   {
                                       NombreCompleto = user.NombreCompleto,
                                       Correo = user.Email,
                                       Rol = role.Name
                                   }).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }

            return userModel;
        }

        /// <summary>
        /// Inicia sesion, si no encuentra el usuario crea nuevo registro
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>string</returns>
        async Task<string> ISeguridad.Login(UsuarioDTO usuario)
        {
            Usuario searchUser = await userManager.FindByEmailAsync(usuario.Correo);
            if (searchUser == null)
            {
                searchUser = await CreateUser(usuario);
            }
            var userRoles = await userManager.GetRolesAsync(searchUser);

            return jwtService.BuildToken(searchUser, userRoles.FirstOrDefault()!);
        }

        /// <summary>
        /// Obtiene los roles de la base de datos
        /// </summary>
        /// <returns>List<RolModel></returns>
        async Task<List<RolDTO>> ISeguridad.Roles()
        {
            var roles = await context.Roles.Select(x => new RolDTO { Nombre = x.Name! }).ToListAsync();
            return roles;
        }

        /// <summary>
        /// Crea nuevo usuario
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns>Usuario</returns>
        /// <exception cref="CustomException"></exception>
        private async Task<Usuario> CreateUser(UsuarioDTO userDTO)
        {
            //Creacion de nuevo usuario
            var newUser = new Usuario()
            {
                UserName = userDTO.Correo,
                Email = userDTO.Correo,
                NombreCompleto = userDTO.NombreCompleto
            };
            var resultadoNuevoUser = await userManager.CreateAsync(newUser);
            if (!resultadoNuevoUser.Succeeded) throw new CustomException(Constantes.ERROR_SEG03);

            //Asignar roles 
            var resultadoRolUser = await userManager.AddToRoleAsync(newUser, Constantes.USUARIO);
            if (!resultadoRolUser.Succeeded) throw new CustomException(Constantes.ERROR_SEG02);

            //Repuesta de usuario creado 
            var user = await userManager.FindByEmailAsync(userDTO.Correo);
            return user;
        }

        public Task<List<RolDTO>> Roles()
        {
            throw new NotImplementedException();
        }
    }
}
