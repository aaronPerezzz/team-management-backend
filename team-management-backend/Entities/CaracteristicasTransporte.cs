using System.ComponentModel.DataAnnotations;

namespace team_management_backend.Entities
{
    public class CaracteristicasTransporte
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Placas { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public int NumeroPuertas { get; set; }
        [Required]
        public string Transmision { get; set; }
        [Required]
        public string Cilindrada { get; set; }
        [Required]
        public string Combustible { get; set; }
        [Required]
        public int AñoCompra { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        public virtual Equipo Equipo { get; set; }
    }

}
