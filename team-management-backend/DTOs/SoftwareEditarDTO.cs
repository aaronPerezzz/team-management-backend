namespace team_management_backend.DTOs
{
    public class SoftwareEditarDTO
    {
        public int? Id { get; set; }
        public string? Serial { get; set; }
        public string? Version { get; set; }
        public DateOnly FechaCompra { get; set; }
    }
}
