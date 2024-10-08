using System.ComponentModel.DataAnnotations;

namespace team_management_backend.domain.Entities
{
    public class TipoEquipo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        // Relaciones
        public ICollection<Equipo> Equipos { get; set; }
    }

}
