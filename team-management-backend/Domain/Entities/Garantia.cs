using System.ComponentModel.DataAnnotations;

namespace team_management_backend.domain.Entities
{
    public class Garantia
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Tipo_Garantia { get; set; }
        [Required]
        public string Proveedor { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }

}
