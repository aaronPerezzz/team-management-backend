﻿using System.ComponentModel.DataAnnotations;

namespace team_management_backend.DTOs
{
    public class SoftwareDTO
    {
        public int? Id { get; set; }
        public string Marca { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string Serial { get; set; }
        public string Version { get; set; }
        public DateOnly FechaCompra { get; set; }
        public DateOnly FechaInstalacion { get; set; }
    }
}
