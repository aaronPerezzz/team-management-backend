using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using team_management_backend.Exceptions;
using team_management_backend.Interfaces;
using team_management_backend.Model;
using team_management_backend.Model.Asignaciones;
using team_management_backend.Utils;
using team_management_backend.Utils.Pagination;

/**
 * @author Alejandro Martínez
 * @since 08/10/2024
 */
namespace team_management_backend.Controller
{
    [Route("api/asignaciones")]
    [ApiController]
    public class AsignacionesController : ControllerBase
    {
        private readonly IAsignacion asignacionService;

        public AsignacionesController(IAsignacion asignacionService)
        {
            this.asignacionService = asignacionService;
        }

        /// <summary>
        /// Obtiene un listado de todas las asignaciones
        /// </summary>
        /// <param name="pag"></param>
        /// <returns>List<AsignacionRegistroDTO></returns>
        [HttpGet]
        public async Task<ActionResult<BaseModel<List<AsignacionRegistroDTO>>>> GetAssignments([FromQuery] PaginationDTO pag)
        {
            BaseModel<List<AsignacionRegistroDTO>> respuesta;
            try
            {
                var asignaciones = await asignacionService.GetAllAssignments(pag);
                return Ok(respuesta = new(true, Constantes.MSJ_AS03, asignaciones));
            }
            catch (CustomException ex)
            {
                return respuesta = new(false, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, respuesta = new(false, Constantes.ERROR_AS07 + ex.HResult));
            }
        }

        /// <summary>
        /// Obtiene un listado de asignaciones 
        /// filtarada por el tipo de equipo
        /// </summary>
        /// <param string tipoEquipo></param>
        /// <param name="pag"></param>
        /// <returns>List<AsignacionRegistroDTO></returns>
        [HttpGet("equipo")]
        public async Task<ActionResult<BaseModel<List<AsignacionRegistroDTO>>>> GetAssignmentsByType([FromQuery] string tipoEquipo, [FromQuery]  PaginationDTO pag)
        {
            BaseModel<List<AsignacionRegistroDTO>> respuesta;

            bool tipoEquipoExiste = await asignacionService.ThereIsEquipment(tipoEquipo);

            if (!tipoEquipoExiste)
            {
                return BadRequest(respuesta = new BaseModel<List<AsignacionRegistroDTO>>(false, Constantes.ERROR_AS08 + tipoEquipo));
            }

            var assignments = await asignacionService.GetAssignmentsByType(tipoEquipo, pag);

            if (assignments == null || !assignments.Any())
            {
                return NotFound(respuesta = new BaseModel<List<AsignacionRegistroDTO>>(false, Constantes.ERROR_AS09));
            }

            return Ok(respuesta = new BaseModel<List<AsignacionRegistroDTO>>(true, Constantes.MSJ_AS03, assignments));
        }


        /// <summary>
        /// Obtiene un listado de asignaciones 
        /// de un usuario
        /// </summary>
        /// <param string correo></param>
        /// <returns>List<AsignacionRegistroDTO></returns>
        [HttpGet("usuario")]
        public async Task<ActionResult<BaseModel<List<AsignacionRegistroDTO>>>> GetUserAssignments([FromQuery] string correo)
        {
            BaseModel<List<AsignacionRegistroDTO>> respuesta;
            try
            {
                var (lstAsignaciones, msj) = await asignacionService.UserAssignment(correo);
                respuesta = new(true, msj, lstAsignaciones);
                return Ok(respuesta);
            }
            catch (CustomException ex)
            {
                return BadRequest(new BaseModel<List<AsignacionRegistroDTO>>(false, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseModel<List<AsignacionRegistroDTO>>(false, Constantes.ERROR_AS10 + ex.Message));
            }
        }

        /// <summary>
        /// Crea una nueva asignación
        /// </summary>
        /// <param name="asignacion"></param>
        /// <returns>List<AsignacionCrearDTO></returns>
        [HttpPost]
        public async Task<ActionResult<BaseModel<int>>> CreateAssignment([FromBody] AsignacionCrearDTO asignacion)
        {
            BaseModel<int> respuestas;
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
                return StatusCode(500, respuestas = new(false, Constantes.ERROR_AS07 + ex.HResult));
            }

        }


        /// <summary>
        /// Modifica una asignación, 
        ///puede modifcar el equipo, si es temporal y las fechas de la asignación
        /// </summary>
        /// <param name="asignacion"></param>
        /// <returns>List<AsignacionRegistroDTO></returns>
        [HttpPut]
        public async Task<ActionResult<BaseModel<int>>> EditarReservacion([FromBody] AsignacionEditarDTO asignacion)
        {
            BaseModel<int> respuestas;
            try
            {
                var res = await asignacionService.UpdateAssignment(asignacion);
                return Ok(respuestas = new(true, res.msj, res.id));
            }
            catch (CustomException ex)
            {
                return respuestas = new(false, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, respuestas = new(false, Constantes.ERROR_AS07 + ex.Message));
            }

        }


        /// <summary>
        /// Elimina una asignación
        /// </summary>
        /// <param int id></param>
        /// <returns>string</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<BaseModel<string>>> DeleteAssignment(int id)
        {
            BaseModel<string> respuestas;
            if (id < 0) return BadRequest(respuestas = new(false, Constantes.ERROR_AS11, default));
            try
            {
                var respuesta = await asignacionService.DeleteAssignment(id);
                return Ok(respuestas = new(true, Constantes.MSJ_AS02, ""));
            }
            catch (CustomException ex)
            {
                return NotFound(respuestas = new(false, ex.Message, default));
            }
            catch (Exception ex)
            {
                return NotFound(respuestas = new(false, Constantes.ERROR_AS07 + ex.Message, default));
            }
        }
    }
}
