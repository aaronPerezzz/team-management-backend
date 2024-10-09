/*
 * @author Aaron Pérez
 * @since 07/10/2024
 */

namespace team_management_backend.Web.Model
{
    public class AutenicacionDTO
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
