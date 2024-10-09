using System.ComponentModel.DataAnnotations;

namespace team_management_backend.Web.Model.Asignaciones
{
    public class AsignacionCrearDTO
    {
        public int IdEquipo { get; set; }
        public string CorreoUsuario { get; set; }
        public bool esTemporal { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public DateTime? FechaFinAsignacion { get; set; }
    }
}
