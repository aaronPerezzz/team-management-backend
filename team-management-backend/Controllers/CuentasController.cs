using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using team_management_backend.DTOs;
using team_management_backend.Entities;
using team_management_backend.Exceptions;
using team_management_backend.Utils;

namespace team_management_backend.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class CuentasController: ControllerBase
    {
        private readonly UserManager<Usuario> userManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;

        public CuentasController(
            UserManager<Usuario> userManager, 
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.configuration = configuration;
         
            this.context = context;
        }

       
        [HttpPost("login")]
        public async Task<ActionResult<(string, string)>> Login([FromBody]UsuarioDTO user)
        {
            var searchUser = await userManager.FindByEmailAsync(user.Correo);
            if (searchUser == null)
            {
                searchUser = await CreateUser(user);
            }
            var userRoles = await userManager.GetRolesAsync(searchUser);
            return (Constants.MSJ_SEG01, BuildToken((Usuario)searchUser, userRoles.FirstOrDefault()!));
        }

        private async Task<List<UsuarioDTO>> ObtenerUsuarios()
        {
            var consulta = await (from user in context.Users
                                  join userRole in context.UserRoles on user.Id equals userRole.UserId
                                  join role in context.Roles on userRole.RoleId equals role.Id
                                  select new UsuarioDTO
                                  {
                                      NombreCompleto = user.NombreCompleto,
                                      Correo = user.Email,
                                      Rol = role.Name
                                  }).ToListAsync();
            return consulta;
        }

        private async Task<Usuario> CreateUser(UsuarioDTO userDTO)
        {
            //Creacion de nuevo usuario
            var newUser = new Usuario() { 
                UserName = userDTO.Correo, 
                Email = userDTO.Correo, 
                NombreCompleto = userDTO.NombreCompleto 
            };
            var resultadoNuevoUser = await userManager.CreateAsync(newUser);
            if (!resultadoNuevoUser.Succeeded) throw new CustomException(Constants.ERROR_SEG03);

            //Asignar roles 
            var resultadoRolUser = await userManager.AddToRoleAsync(newUser, Constants.USUARIO);
            if (!resultadoRolUser.Succeeded) throw new CustomException(Constants.ERROR_SEG02);

            //Repuesta de usuario creado 
            var user = await userManager.FindByEmailAsync(userDTO.Correo);
            return user;
        }

        private async Task<string> EditRol(UsuarioDTO usuario)
        {
            //Buscar al usuario
            var usuarioBD = await userManager.FindByEmailAsync(usuario.Correo);
            if (usuarioBD == null) throw new CustomException(Constants.ERROR_SEG01);

            //buscar los roles antiguos del usuario
            var rolAntiguo = await userManager.GetRolesAsync(usuarioBD);
            if (rolAntiguo is not null)
            {
                //Remover el rol anterior
                foreach (string rolesActuales in rolAntiguo)
                {
                    await userManager.RemoveFromRoleAsync(usuarioBD, rolesActuales);
                }
            }

            var AgregarRol = await userManager.AddToRoleAsync(usuarioBD, usuario.Rol!);
            if (!AgregarRol.Succeeded) throw new CustomException(Constants.ERROR_SEG02);

            return Constants.MSJ_SEG02;
        }

        private async Task<List<RolDTO>> Roles()
        {
            var roles = await context.Roles.Select(x => new RolDTO { Nombre = x.Name! }).ToListAsync();
            return roles;
        }

        private string BuildToken(Usuario user, string rol) 
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, rol),
                new Claim(ClaimTypes.Name, user.NombreCompleto)
            };

            var jwt = configuration.GetSection("jwt").Get<JwtDTO>();
            var keyToken = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
            var credentialsLogin = new SigningCredentials(keyToken, SecurityAlgorithms.HmacSha256);
            var expirationToken = DateTime.UtcNow.AddMinutes(30);

            var securityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: null,
                claims: claims, 
                expires: expirationToken, 
                signingCredentials: credentialsLogin);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
