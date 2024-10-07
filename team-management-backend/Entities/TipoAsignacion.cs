using System.ComponentModel.DataAnnotations;

namespace team_management_backend.Entities
{
    public class TipoAsignacion
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
        public List<Asignacion> Asignaciones { get; set; }
    }

}
