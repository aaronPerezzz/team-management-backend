using System.ComponentModel.DataAnnotations;

namespace team_management_backend.domain.Entities
{
    public class Poliza
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Aseguradora { get; set; }
        [Required]
        public string Numero_poliza { get; set; }
        [Required]
        public string Cobertura { get; set; }
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
