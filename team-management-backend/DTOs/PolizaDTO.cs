using System.ComponentModel.DataAnnotations;

namespace team_management_backend.DTOs
{
    public class PolizaDTO
    {
        public int? Id { get; set; }
        public string Aseguradora { get; set; }
        public string Numero_poliza { get; set; }
        public string Cobertura { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
    }
}
