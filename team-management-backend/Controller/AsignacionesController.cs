using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using team_management_backend.Domain.Interfaces.Repository;
using team_management_backend.Domain.Interfaces.Service;
using team_management_backend.Exceptions;
using team_management_backend.Web.Model;
using team_management_backend.Web.Model.Asignaciones;

/**
 * @author Alejandro Martínez
 * @since 08/10/2024
 */
namespace team_management_backend.Web.Controller
{
    [Route("api/asignaciones")]
    [ApiController]
    public class AsignacionesController: ControllerBase
    {
        private readonly IAsignacion asignacionService;

        public AsignacionesController(IAsignacion asignacionService)
        {
            this.asignacionService = asignacionService;
        }

        /// <summary>
        /// Obtiene un listado de todas las asignaciones
        /// </summary>
        /// <returns>List<AsignacionRegistroDTO></returns>
        [HttpGet]
        public async Task<ActionResult<BaseDTO<List<AsignacionRegistroDTO>>>> GetAssignments()
        {
            BaseDTO<List<AsignacionRegistroDTO>> respuesta;
            try
            {
                var asignaciones = await asignacionService.GetAllAssignments();
                return Ok(respuesta = new(true, "exito", asignaciones));
            }
            catch (CustomException ex)
            {
                return respuesta = new(false, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, respuesta = new(false, "Error no controlado en el servicio de asignaciones " + ex.HResult));
            }
        }

        /// <summary>
        /// Obtiene un listado de asignaciones 
        /// filtarada por el tipo de equipo
        /// </summary>
        /// <returns>List<AsignacionRegistroDTO></returns>
        [HttpGet("equipo")]
        public async Task<ActionResult<BaseDTO<List<AsignacionRegistroDTO>>>> GetAssignmentsByType([FromQuery] string tipoEquipo)
        {
            BaseDTO<List<AsignacionRegistroDTO>> respuesta;

            bool tipoEquipoExiste = await asignacionService.ExisteTipoEquipo(tipoEquipo);

            if (!tipoEquipoExiste)
            {
                return BadRequest(respuesta = new BaseDTO<List<AsignacionRegistroDTO>>(false, $"El tipo de equipo '{tipoEquipo}' no existe."));
            }

            var assignments = await asignacionService.GetAssignmentsByType(tipoEquipo);

            if (assignments == null || !assignments.Any())
            {
                return NotFound(respuesta = new BaseDTO<List<AsignacionRegistroDTO>>(false, "No se encontraron asignaciones para el tipo de equipo proporcionado."));
            }

            return Ok(respuesta = new BaseDTO<List<AsignacionRegistroDTO>>(true, "Éxito", assignments));
        }

        [HttpGet("usuario")]
        public async Task<ActionResult<BaseDTO<List<AsignacionRegistroDTO>>>> GetUserAssignments([FromQuery]string correo)
        {
            BaseDTO<List<AsignacionRegistroDTO>> respuesta;

            try
            {
                var (lstAsignaciones, msj) = await asignacionService.UserAssignment(correo);
                respuesta = new(true, msj, lstAsignaciones);
                return Ok(respuesta);
            }
            catch (CustomException ex)
            {
                return BadRequest(new BaseDTO<List<AsignacionRegistroDTO>>(false, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseDTO<List<AsignacionRegistroDTO>>(false, "Error no controlado: " + ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseDTO<int>>> CreateAssignment([FromBody] AsignacionCrearDTO asignacion)
        {
            BaseDTO<int> respuestas;
            try
            {
                var res = await asignacionService.CreateAssignment(asignacion);
                return Ok(respuestas = new(true, res.msj, res.id));
            }
            catch (CustomException ex)
            {
                return respuestas = new(false, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, respuestas = new(false,"Error no controlado en servicios Asignación" + ex.HResult));
            }

        }
    }
}
