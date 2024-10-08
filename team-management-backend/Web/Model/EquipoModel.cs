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
        public int IdGarantia { get; set; }
        [Required]
        public int IdPoliza { get; set; }
        [Required]
        public string Marca { get; set; }
        [Required]
        public string Modelo { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public DateTime FechaCompra { get; set; }
       
    }
}
