using System.ComponentModel.DataAnnotations;

namespace team_management_backend.Models
{
    public class CaracteristicasTransporte
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdEquipo { get; set; }

        [Required]
        public string? Placas { get; set; }

        [Required]
        public string? Color { get; set; }

        public int? NumeroPuertas { get; set; }

        [Required]
        public string? Transmision { get; set; }

        public string? Cilindrada { get; set; }

        public string? Combustible { get; set; }
        public string? IdUsuarioCreacion { get; set; }
        public string? IdUsuarioModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        //Relaciones
        public Equipo Equipo { get; set; }
    }

}
