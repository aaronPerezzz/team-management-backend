using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using team_management_backend.Context;
using team_management_backend.Exceptions;
using team_management_backend.Interface;
using team_management_backend.DTOs;
using team_management_backend.Utils;
using team_management_backend.Models;


/**
 * @author Aaron Pérez
 * @since 07/10/2024
 */
namespace team_management_backend.Controller
{
    [ApiController]
    [Route("api/security")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SeguridadController : ControllerBase
    {
        private readonly UserManager<Usuario> userManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly ISeguridad seguridadService;

        public SeguridadController(
            UserManager<Usuario> userManager,
            IConfiguration configuration,
            ApplicationDbContext context,
            ISeguridad seguridadService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.context = context;
            this.seguridadService = seguridadService;
        }


        /// <summary>
        /// Iniciar sesion con correo, si no existe el correo crea un nuevo registro
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login([FromBody]UsuarioDTO user)
        {
            BaseDTO<string> response;
            try
            {
                string tokenResult = await seguridadService.Login(user);
                return Ok(response = new(true, Constantes.MSJ_SEG01, tokenResult));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, response = new(false, ex.Message));
            }
        }

        /// <summary>
        /// Consulta todos los usuarios de la base de datos
        /// </summary>
        /// <returns>List<UsuarioModel></returns>
        [HttpGet("users")]
        public async Task<ActionResult<List<UsuarioDTO>>> GetAllUsers()
        {
            BaseDTO<List<UsuarioDTO>> users;
            List<UsuarioDTO> getAllUsers = new List<UsuarioDTO> ();
            try
            {
                getAllUsers = await seguridadService.GetAllUsers();
                if (getAllUsers.Count < 1)
                {
                    return NotFound(users = new(true, Constantes.ERROR_SEG05, getAllUsers));
                }
                return Ok(users = new(true, Constantes.MSJ_SEG04, getAllUsers));
            }
            catch (CustomException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, users = new(false, ex.Message));
            }
        }

        [HttpGet("users/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<UsuarioDTO>> getUserById(string id)
        {
            UsuarioDTO userModel = new UsuarioDTO();
            BaseDTO<UsuarioDTO> user;
            try
            {
                userModel = await seguridadService.GetUserById(id);
                if (userModel is null)
                {
                    return NotFound(user = new (Constantes.FALSE, "Usuario no existe", new UsuarioDTO()));
                }
            }
            catch (CustomException ex) {
                return StatusCode((int)HttpStatusCode.InternalServerError, user = new(false, ex.Message));
            }
            return Ok(user = new (Constantes.TRUE, "Usuario", userModel));
        }


        /// <summary>
        /// Modifica el rol de un usuario
        /// </summary>
        /// <param name="usuarioModel"></param>
        /// <returns>ActionResult<string></returns>
        [HttpPut("roles")]
        public async Task<ActionResult<BaseDTO<string>>> EditRol(UsuarioDTO usuarioModel)
        {
            BaseDTO<string> responseRol;
            try
            {
                string changeRol = await seguridadService.EditRol(usuarioModel);
                return Ok(responseRol = new(Constantes.TRUE, Constantes.MSJ_SEG02));
            }
            catch (CustomException ex)
            {
                return responseRol = new(Constantes.FALSE, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, responseRol = new(false, ex.Message));
            }
        }

        /// <summary>
        /// Obtiene todos los roles
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        public async Task<ActionResult<List<RolDTO>>> GetRoles()
        {
            BaseDTO<List<RolDTO>> rolesList;
            try
            {
                List<RolDTO> roles = await seguridadService.Roles();
                if (roles.Count < 1)
                {
                    return BadRequest(rolesList = new(Constantes.FALSE, Constantes.ERROR_SEG04));
                }
                return Ok(rolesList = new(Constantes.TRUE, Constantes.MSJ_SEG03, roles));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, rolesList = new(false, ex.Message));
            }
        }


    }
}
