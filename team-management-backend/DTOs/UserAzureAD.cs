using System.Security.Claims;

namespace team_management_backend.DTOs
{
    public class UserAzureAD
    {
        public string user_name { get; set; }
        public string user_domain { get; set; }
        public string user_email { get; set; }
    }


}
