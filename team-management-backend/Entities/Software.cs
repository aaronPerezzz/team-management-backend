using System.ComponentModel.DataAnnotations;

namespace team_management_backend.Entities
{
    public class Software
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
        [Required]
        public string Version { get; set; }
        [Required]
        public DateTime FechaCompra { get; set; }
        [Required]
        public DateTime FechaInstalacion { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        public List<EquipoSoftware> EquiposSoftware { get; set; }
    }

}