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
      
       
        private readonly IEquipos equiposService;
        private readonly IMapper mapper;

        public EquiposController(
            IEquipos equiposService,
            IMapper mapper)
        {
            this.equiposService = equiposService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<EquipoModel>>> GetAll()
        {
            BaseModel<List<Equipo>> response;
            List<Equipo> equipoList;
            try
            {
                equipoList = await equiposService.GetEquipmentAll();
                if (equipoList.Count < 1)
                {
                    return NotFound(response = new(Constantes.FALSE, "No hay información"));
                }
            }
            catch (CustomException ex) 
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, response = new(Constantes.FALSE, ex.Message));
            }
            return Ok(response = new(Constantes.TRUE, "Listado de equipos", mapper.Map<List<Equipo>>( equipoList)));
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
