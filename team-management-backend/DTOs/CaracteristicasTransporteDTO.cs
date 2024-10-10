using System.ComponentModel.DataAnnotations;

namespace team_management_backend.DTOs
{
    public class CaracteristicasTransporteDTO
    {
        public int? Id { get; set; }
        public int? IdEquipo { get; set; }
        public string Placas { get; set; }
        public string Color { get; set; }
        public int? NumeroPuertas { get; set; }
        public string Transmision { get; set; }
        public string? Cilindrada { get; set; }
        public string? Combustible { get; set; }
    }
}
