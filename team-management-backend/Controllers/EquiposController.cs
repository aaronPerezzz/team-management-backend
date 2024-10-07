using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using team_management_backend.Entities;

namespace team_management_backend.Controllers
{
    [Route("api/equipos")]
    [ApiController]
    [Authorize]
    public class EquiposController: ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Equipo>> GetAll()
        {
            return new List<Equipo>();
        }

    }
}
