using System.ComponentModel.DataAnnotations;

namespace team_management_backend.Web.Model
{
    public class PolizaModel
    {
        public int? Id { get; set; }
        public string Aseguradora { get; set; }
        public string Numero_poliza { get; set; }
        public string Cobertura { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
