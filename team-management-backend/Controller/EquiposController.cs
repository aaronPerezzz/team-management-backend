using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        [HttpPost]
        public async Task<ActionResult<BaseModel<EquipoModel>>> PostAll([FromBody] EquipoModel equipo)
        {
            BaseModel<EquipoModel> response;
            Equipo model;

            try
            {
                model = await equiposService.SaveEquipment(equipo);
            }
            catch (CustomException ex) {
                return StatusCode((int)HttpStatusCode.InternalServerError, response = new(Constantes.FALSE, ex.Message));
            }

           return Ok(response = new(Constantes.TRUE, "Guardado con exito", mapper.Map<EquipoModel>(model)));
        }



    }
}
