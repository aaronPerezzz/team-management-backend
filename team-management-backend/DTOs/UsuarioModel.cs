using System.ComponentModel.DataAnnotations;

/*
 * @author Aaron Pérez
 * @since 07/10/2024
 */
namespace team_management_backend.Model
{
    public class UsuarioModel
    {
        [EmailAddress]
        [Required]
        public string Correo { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
    }
}
