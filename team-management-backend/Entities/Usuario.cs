using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace team_management_backend.Entities
{
    public class Usuario: IdentityUser
    {
        [Required]
        public string NombreCompleto { get; set; }
    }
}
