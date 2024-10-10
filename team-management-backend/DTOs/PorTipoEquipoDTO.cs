using team_management_backend.Web.Model;

namespace team_management_backend.DTOs
{
    public class PorTipoEquipoDTO: TipoEquipoDTO
    {
        public List<EquipoDTO> equipos { get; set; }
    }
}
