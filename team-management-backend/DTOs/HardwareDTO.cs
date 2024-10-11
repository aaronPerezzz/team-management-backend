using System.ComponentModel.DataAnnotations;

namespace team_management_backend.DTOs
{
    public class HardwareDTO
    {
        public int? Id { get; set; }
        public string? Marca { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Serial { get; set; }
    }
}
