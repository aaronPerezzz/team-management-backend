using System.ComponentModel.DataAnnotations;
using team_management_backend.domain.Entities;

namespace team_management_backend.Web.Model
{
    public class EquipoModel
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
        public DateTime FechaCompra { get; set; }
        public PolizaModel? Poliza { get; set; }
        public GarantiaModel? Garantia { get; set; }
        public CaracteristicasTransporteModel? CaracteristicasTransporte { get; set; }
        public List<SoftwareModel>? Software { get; set; }
        public List<HardwareModel>? Hardware { get; set; }

    }
}
