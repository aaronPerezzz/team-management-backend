using System.ComponentModel.DataAnnotations;

namespace team_management_backend.DTOs
{
    public class CredencialesUsuarioDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
