namespace team_management_backend.DTOs
{
    public class JwtDTO
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
    }
}
