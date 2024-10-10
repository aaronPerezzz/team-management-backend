using System.ComponentModel.DataAnnotations;

namespace team_management_backend.Models
{
    public class Hardware
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Marca { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Serial { get; set; }

        public string IdUsuarioCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }


        public List<EquipoHardware> EquiposHardware { get; set; }
    }

}
