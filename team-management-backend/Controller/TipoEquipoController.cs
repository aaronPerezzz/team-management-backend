using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using team_management_backend.domain.Entities;
using team_management_backend.Domain.Interfaces.Repository;
using team_management_backend.Utils;
using team_management_backend.Web.Model;

/*
 * @author Aaron Pérez
 * @since 08/10/2024
 */
namespace team_management_backend.Web.Controller
{
    [Route("api/tipo-equipo")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Constantes.ADMINISTRADOR}")]
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
        public async Task<ActionResult<BaseModel<List<TipoEquipoModel>>>> GetAll()
        {
            List<TipoEquipo> typeEquitmentResponse = new List<TipoEquipo>();
            BaseModel<List<TipoEquipoModel>> typeEquitmentModel;
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

            return Ok(typeEquitmentModel = new(Constantes.TRUE, Constantes.MSJ_TIE01, mapper.Map<List<TipoEquipoModel>>(typeEquitmentResponse)));
        }
    }
}
