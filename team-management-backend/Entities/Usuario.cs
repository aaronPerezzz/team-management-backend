using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


/*
 * @author Aaron Pérez
 * @since 07/10/2024
 */
namespace team_management_backend.domain.Entities
{
    public class Usuario : IdentityUser
    {
        [Required]
        public string NombreCompleto { get; set; }
    }
}
