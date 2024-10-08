using System.ComponentModel.DataAnnotations;

namespace team_management_backend.domain.Entities
{
    public class Equipo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IdTipoEquipo { get; set; }
        [Required]
        public int IdGarantia { get; set; }
        public int IdPoliza { get; set; }
        [Required]
        public string Marca { get; set; }
        [Required]
        public string Modelo { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public DateTime FechaCompra { get; set; }

        public virtual CaracteristicasTransporte CaracteristicasTransporte { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        // Relaciones
        public TipoEquipo TipoEquipo { get; set; }
        public Garantia Garantia { get; set; }
        public Poliza Poliza { get; set; }

        public List<EquipoHardware> EquiposHardware { get; set; }
        public List<EquipoSoftware> EquiposSoftware { get; set; }
    }


}
