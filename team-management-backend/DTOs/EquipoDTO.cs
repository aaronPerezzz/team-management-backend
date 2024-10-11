using System.ComponentModel.DataAnnotations;


namespace team_management_backend.DTOs
{
    public class EquipoDTO
    {
        public int? Id { get; set; }
        [Required]
        public int IdTipoEquipo { get; set; }
        [Required]
        public string Marca { get; set; }
        [Required]
        public string Modelo { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public string CorreoUsuario { get; set; }
        [Required]
        public string Serial { get; set; }
        [Required]
        public DateOnly FechaCompra { get; set; }
        public PolizaDTO? Poliza { get; set; }
        public GarantiaDTO? Garantia { get; set; }
        public CaracteristicasTransporteDTO? CaracteristicasTransporte { get; set; }
        public List<SoftwareDTO>? Software { get; set; }
        public List<HardwareDTO>? Hardware { get; set; }

    }
}
