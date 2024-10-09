using System.ComponentModel.DataAnnotations;

namespace team_management_backend.Web.Model
{
    public class GarantiaModel
    {
        public int? Id { get; set; }
        public string Tipo_Garantia { get; set; }
        public string Proveedor { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
