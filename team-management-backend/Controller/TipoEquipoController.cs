using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using team_management_backend.Models;
using team_management_backend.Interface;
using team_management_backend.Utils;
using team_management_backend.DTOs;

/*
 * @author Aaron Pérez
 * @since 08/10/2024
 */
namespace team_management_backend.Web.Controller
{
    [Route("api/tipos-equipos")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Constantes.ADMINISTRADOR}")]
    public class TipoEquipoController : ControllerBase
    {
        private readonly ITipoEquipo tipoEquipoService;
        private readonly IMapper mapper;

        public TipoEquipoController(
            ITipoEquipo tipoEquipoService,
            IMapper mapper)
        {
            this.tipoEquipoService = tipoEquipoService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Obtiene listado de tipos de equipo
        /// </summary>
        /// <returns>List<TipoEquipoModel></returns>

        [HttpGet]
        public async Task<ActionResult<BaseDTO<List<TipoEquipoDTO>>>>GetAll()
        {
            List<TipoEquipoDTO> typeEquitmentResponse = new List<TipoEquipoDTO>();
            BaseDTO<List<TipoEquipoDTO>> typeEquitmentModel;
            try
            {
                typeEquitmentResponse = await tipoEquipoService.GetAll();
                if (typeEquitmentResponse.Count < Constantes.NUM0)
                {
                    return NotFound(typeEquitmentModel = new(Constantes.FALSE, Constantes.ERROR_TIE01));
                }

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, typeEquitmentModel = new(Constantes.FALSE, ex.Message));
            }

            return Ok(typeEquitmentModel = new(Constantes.TRUE, Constantes.MSJ_TIE01, typeEquitmentResponse));
        }
    }
}
