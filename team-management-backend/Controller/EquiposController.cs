using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using team_management_backend.domain.Entities;
using team_management_backend.Domain.Interfaces.Repository;
using team_management_backend.DTOs;
using team_management_backend.Exceptions;
using team_management_backend.Utils;
using team_management_backend.Web.Model;


namespace team_management_backend.Controllers
{
    [Route("api/equipos")]
    [ApiController]

    public class EquiposController : ControllerBase
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
        public async Task<ActionResult<List<EquipoDTO>>> GetAll()
        {
            BaseDTO<List<EquipoDTO>> response;
            List<EquipoDTO> equipoList;
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
            return Ok(response = new(Constantes.TRUE, "Listado de equipos", equipoList));
        }

        [HttpPost]
        public async Task<ActionResult<BaseDTO<EquipoDTO>>> PostTypeEquipment([FromBody] EquipoDTO equipo)
        {
            BaseDTO<EquipoDTO> response;
            EquipoDTO model;

            try
            {
                model = await equiposService.SaveEquipment(equipo);
            }
            catch (CustomException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, response = new(Constantes.FALSE, ex.Message));
            }

            return Ok(response = new(Constantes.TRUE, "Guardado con exito", model));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BaseDTO<EquipoDTO>>> GetById(int id)
        {
            BaseDTO<EquipoDTO> responseEquipment;
            EquipoDTO equipment;
            try
            {
                equipment = await equiposService.GetFindById(id);
                if(equipment is null){
                    return NotFound(responseEquipment = new(Constantes.FALSE, "No se encontro información"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, responseEquipment = new(Constantes.FALSE, ex.Message));
            }
            return Ok(responseEquipment = new(Constantes.TRUE, "Equipo", equipment));
        }

        [HttpGet("{typeEquipment}")]
        [DefaultValue("Transporte")]
        public async Task<ActionResult<BaseDTO<List<PorTipoEquipoDTO>>>> GetByTypeEquipment(string typeEquipment)
        {
            BaseDTO<List<PorTipoEquipoDTO>> responseTypeEquipment;
            List<PorTipoEquipoDTO> resultEquipment;
            try {
                resultEquipment = await equiposService.GetFindByType(typeEquipment);
            } catch(Exception ex){
                return StatusCode((int)HttpStatusCode.InternalServerError, responseTypeEquipment = new(Constantes.FALSE, ex.Message));
            }
            return Ok(responseTypeEquipment = new(Constantes.TRUE, "Tipos de equipos", resultEquipment));
        }
    }
}
