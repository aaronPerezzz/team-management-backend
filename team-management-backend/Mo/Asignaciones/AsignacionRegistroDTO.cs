namespace team_management_backend.Model.Asignaciones
{
    public class AsignacionRegistroDTO
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string TipoEquipo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public bool esTemporal { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public DateTime? FechaFinAsignacion { get; set; }
    }
}
