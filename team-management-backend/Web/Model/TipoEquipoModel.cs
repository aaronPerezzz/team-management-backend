using System.ComponentModel.DataAnnotations;

namespace team_management_backend.Web.Model
{
    public class TipoEquipoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
