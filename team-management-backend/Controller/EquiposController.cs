using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using team_management_backend.Context;
using team_management_backend.DTOs;
using team_management_backend.Exceptions;
using team_management_backend.Interface;
using team_management_backend.Models;
using team_management_backend.Utils;
using team_management_backend.Utils.Pagination;



namespace team_management_backend.Controllers
{
    [Route("api/equipos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EquiposController : ControllerBase
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEquipos equiposService;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;

        public EquiposController(
            IEquipos equiposService,
            IMapper mapper,
            ApplicationDbContext context)
        {
            this.equiposService = equiposService;
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<EquipoDTO>>> GetAll([FromQuery] PaginationDTO pagination)
        {
            BaseDTO<List<EquipoDTO>> response;
            List<EquipoDTO> equipoList;
            try
            {
                equipoList = await equiposService.GetEquipmentAll(pagination);
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
                if (equipment is null)
                {
                    return NotFound(responseEquipment = new(Constantes.FALSE, "No se encontro información"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, responseEquipment = new(Constantes.FALSE, ex.Message));
            }
            return Ok(responseEquipment = new(Constantes.TRUE, "Equipo", equipment));
        }

        [HttpGet("{tipo}")]
        public async Task<ActionResult<BaseDTO<List<EquipoDTO>>>> GetByTypeEquipment(string tipo, [FromQuery] PaginationDTO pagination)
        {
            BaseDTO<List<EquipoDTO>> responseTypeEquipment;
            List<EquipoDTO> resultEquipment;
            try
            {
                IQueryable<TipoEquipo> findtypeEquipment = context.TiposEquipo.Where(t => t.Nombre == tipo);
                if (findtypeEquipment.Count() < 1)
                {
                    return NotFound(responseTypeEquipment = new(Constantes.FALSE, "No se encontro información"));
                }
                resultEquipment = await equiposService.GetFindByType(tipo, pagination);
                if (resultEquipment.Count() < 1)
                {
                    return NotFound(responseTypeEquipment = new(Constantes.FALSE, "No se encontro información"));
                }

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, responseTypeEquipment = new(Constantes.FALSE, ex.Message));
            }
            return Ok(responseTypeEquipment = new(Constantes.TRUE, "Tipos de equipos", resultEquipment));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            try
            {
                BaseDTO<ActionResult> responeEquipment;
                Equipo equipment = await context.Equipos.FindAsync(id);
                if (equipment is null || !equipment.Estatus)
                {

                    return NotFound(responeEquipment = new(Constantes.FALSE, "No se encontro el registro"));
                }
                await equiposService.DeleteById(id, equipment);
                return NoContent();
            }
            catch (CustomException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateEquipment(int id, [FromBody] EditarEquipoDTO equipo)
        {
            try
            {
                BaseDTO<ActionResult> responeEquipment;
                Equipo equipmentEntity = await context.Equipos.FindAsync(id);
                if (equipmentEntity is null || !equipmentEntity.Estatus)
                {
                    return NotFound(responeEquipment = new(Constantes.FALSE, "No se encontro el registro"));
                }
                await equiposService.UpdateEquipmentById(id, equipmentEntity, equipo);
                return NoContent();
            }
            catch (CustomException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
