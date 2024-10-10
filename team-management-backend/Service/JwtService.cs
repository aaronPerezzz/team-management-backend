using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using team_management_backend.Models;
using team_management_backend.DTOs;

namespace team_management_backend.Service
{
    public class JwtService
    {
        private readonly IConfiguration configuration;

        public JwtService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string BuildToken(Usuario user, string rol)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, rol),
                new Claim(ClaimTypes.Name, user.NombreCompleto)
            };

            var jwt = configuration.GetSection("jwt").Get<JwtModel>();
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
