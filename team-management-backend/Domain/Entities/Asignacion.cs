using System.ComponentModel.DataAnnotations;

namespace team_management_backend.domain.Entities
{
    public class Asignacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdEquipo { get; set; }
    
        [Required]
        public string IdUsuario { get; set; } = null!;

        public bool esTemporal { get; set; }

        public DateTime? FechaAsignacion { get; set; }
        public DateTime? FechaFinAsignacion { get; set; }
        public string? IdUsuarioCreacion { get; set; }
        public string? IdUsuarioModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        // Relaciones
        public Equipo Equipo { get; set; }
    }

}
