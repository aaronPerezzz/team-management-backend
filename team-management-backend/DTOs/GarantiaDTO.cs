using System.ComponentModel.DataAnnotations;

namespace team_management_backend.DTOs
{
    public class GarantiaDTO
    {
        public int? Id { get; set; }
        public string Tipo_Garantia { get; set; }
        public string Proveedor { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
