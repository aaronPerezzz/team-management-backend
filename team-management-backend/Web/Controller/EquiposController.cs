using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using team_management_backend.domain.Entities;
using team_management_backend.Domain.Interfaces.Repository;
using team_management_backend.Exceptions;
using team_management_backend.Utils;
using team_management_backend.Web.Model;


namespace team_management_backend.Controllers
{
    [Route("api/equipos")]
    [ApiController]
    public class EquiposController: ControllerBase
    {
      
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEquipos equiposService;

        public EquiposController(
            IHttpContextAccessor httpContextAccessor,
            IEquipos equiposService)
        {
          
            this.httpContextAccessor = httpContextAccessor;
            this.equiposService = equiposService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Equipo>>> GetAll()
        {
            BaseModel<List<Equipo>> response;
            Task<List<Equipo>> equipoList;
            try
            {
                equipoList = equiposService.GetEquipmentAll();
            }
            catch (CustomException ex) 
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, response = new(Constantes.FALSE, ex.Message));
            }
            return Ok(response = new(Constantes.TRUE, "Ok", equipoList.Result));
        }

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
