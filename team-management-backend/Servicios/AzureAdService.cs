using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using team_management_backend.DTOs;

namespace team_management_backend.Servicios
{
    public class AzureAdService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AzureAdService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public UserAzureAD GetUserOnAzureAd(ClaimsPrincipal claims)
        {
            var preferredUsernameClaim = claims.Claims.FirstOrDefault(claim => claim.Type.Equals("preferred_username"));
            if (preferredUsernameClaim != null)
            {
                return new UserAzureAD
                {
                    user_name = claims.Claims.FirstOrDefault(name => name.Type.Equals("name")).Value,
                    user_email = preferredUsernameClaim.Value,
                    user_domain = string.Format(@"cpiccr\{0}", preferredUsernameClaim.Value.Split("@")[0])
                };
            }
            return null;
        }

        public ClaimsPrincipal GetInfoUser()
        {
            return _httpContextAccessor.HttpContext?.User;
        }

    }
}
