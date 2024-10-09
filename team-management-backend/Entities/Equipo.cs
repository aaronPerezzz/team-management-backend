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
        public string Marca { get; set; }

        [Required]
        public string Modelo { get; set; }

        [Required]
        public string Estado { get; set; }

        [Required]
        public string Serial { get; set; }

        [Required]
        public DateOnly FechaCompra { get; set; }

        public string? IdUsuarioCreacion { get; set; }

        public string? IdUsuarioModificacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        // Relaciones
        public TipoEquipo TipoEquipo { get; set; }
        public Garantia Garantia { get; set; }
        public Poliza Poliza { get; set; }
        public CaracteristicasTransporte CaracteristicasTransporte { get; set; }
        public List<Software> Software { get; set; }
        public List<Hardware> Hardware { get; set; }
    }


}