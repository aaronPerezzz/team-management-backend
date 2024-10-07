using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace team_management_backend.DTOs
{
    public class UsuarioDTO
    {
        [EmailAddress]
        [Required]
        public string Correo { get; set; }
        [Required]
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
    }
}
