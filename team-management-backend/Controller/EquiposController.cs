using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace team_management_backend.Controller
{
    [Route("api/equipos")]
    [ApiController]
    [Authorize]
    public class EquiposController : ControllerBase
    {

        private readonly IHttpContextAccessor httpContextAccessor;

        public EquiposController(IHttpContextAccessor httpContextAccessor)
        {

            this.httpContextAccessor = httpContextAccessor;
        }

        //[HttpGet]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Read")]
        //public ActionResult<List<Equipo>> GetAll()
        //{

        //    return new List<Equipo> {
        //        new Equipo { Descripcion= "Laptop", Nombre = "Dell"},
        //        new Equipo { Descripcion= "Laptop Lenovo", Nombre = "Lenovo"}
        //    };
        //}

        //[HttpPost]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Read")]
        //public ActionResult<List<Equipo>> PostAll([FromBody] Equipo equipo)
        //{

        //    return new List<Equipo> {
        //        new Equipo { Descripcion= "Laptop", Nombre = "Dell"},
        //        new Equipo { Descripcion= "Laptop Lenovo", Nombre = "Lenovo"},
        //        new Equipo {Descripcion = equipo.Descripcion, Nombre = equipo.Nombre}
        //    };
        //}



    }
}
