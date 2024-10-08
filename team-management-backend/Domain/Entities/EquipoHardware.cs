using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace team_management_backend.domain.Entities
{
    public class EquipoHardware
    {
        public int IdEquipo { get; set; }
        public int IdHardware { get; set; }
        public string? IdUsuarioCreacion { get; set; }
        public string? IdUsuarioModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }


        public Equipo Equipo { get; set; }
        public Hardware Hardware { get; set; }
    }

}
