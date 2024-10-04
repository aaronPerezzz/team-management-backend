using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using team_management_backend.DTOs;

namespace team_management_backend.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public CuentasController(
            UserManager<IdentityUser> userManager, 
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        [HttpPost]
        public async Task<ActionResult<AutenticacionDTO>> RegisterUser([FromBody]CredencialesUsuarioDTO user)
        {
            var userModel = new IdentityUser {
                UserName = user.Email,
                Email = user.Email };
            var resultado = await userManager.CreateAsync(userModel, user.Password);
            if (resultado.Succeeded)
            {
                return BuildToken(user);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AutenticacionDTO>> Login([FromBody]CredencialesUsuarioDTO credentials)
        {
            var result = await signInManager.PasswordSignInAsync(
                credentials.Email,
                credentials.Password,
                isPersistent: false, 
                lockoutOnFailure: false);

            if (result.Succeeded) 
            {
                return BuildToken(credentials);
            } else
            {
                return BadRequest("Usuario/Contraseña incorrectos");
            }
        }

        [HttpGet("renovar-token")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<AutenticacionDTO> RenewToken()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var userCredentials = new CredencialesUsuarioDTO()
            {
                Email = email
            };

            return BuildToken(userCredentials);
        }

        private AutenticacionDTO BuildToken( CredencialesUsuarioDTO credentials) 
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credentials.Email),
            };

            var keyToken = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
            var credentialsLogin = new SigningCredentials(keyToken, SecurityAlgorithms.HmacSha256);

            var expirationToken = DateTime.UtcNow.AddMinutes(30);
            var securityToken = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims, 
                expires: 
                expirationToken, 
                signingCredentials: credentialsLogin);

            return new AutenticacionDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expirationToken
            };
        }
    }
}
