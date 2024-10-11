using System.ComponentModel.DataAnnotations;

namespace team_management_backend.DTOs
{
    public class EditarEquipoDTO
    {
   
        public int IdTipoEquipo { get; set; }
        public string? Estado { get; set; }
        public PolizaDTO? Poliza { get; set; }
        public CaracteristicasTransporteEditarDTO? CaracteristicasTransporte { get; set; }
        public List<SoftwareEditarDTO>? Software { get; set; }
        public List<HardwareDTO>? Hardware { get; set; }
    }
}
