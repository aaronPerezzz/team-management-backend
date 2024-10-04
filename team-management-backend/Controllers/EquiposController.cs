using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Security.Claims;
using team_management_backend.Entities;
using team_management_backend.Servicios;

namespace team_management_backend.Controllers
{
    [Route("api/equipos")]
    [ApiController]
    [Authorize]
    public class EquiposController: ControllerBase
    {
        private readonly AzureAdService _azureAdService;

        public EquiposController(AzureAdService azureAdService)
        {
            this._azureAdService = azureAdService;
        }

        [HttpGet]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Read")]
        public ActionResult<List<Equipo>> GetAll()
        {

            return new List<Equipo> {
                new Equipo { Descripcion= "Laptop", Nombre = "Dell"},
                new Equipo { Descripcion= "Laptop Lenovo", Nombre = "Lenovo"}
            };
        }

        [HttpPost]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public ActionResult<List<Equipo>> PostAll([FromBody] Equipo equipo)
        {

            return new List<Equipo> {
                new Equipo { Descripcion= "Laptop", Nombre = "Dell"},
                new Equipo { Descripcion= "Laptop Lenovo", Nombre = "Lenovo"},
                new Equipo {Descripcion = equipo.Descripcion, Nombre = equipo.Nombre}
            };
        }

        [HttpGet("user")]
        public ActionResult<ClaimsPrincipal> GetClaims()
        {
            return _azureAdService.GetInfoUser();
        }

    }
}
