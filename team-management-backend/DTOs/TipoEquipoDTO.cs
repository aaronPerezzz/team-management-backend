﻿using System.ComponentModel.DataAnnotations;

namespace team_management_backend.DTOs
{
    public class TipoEquipoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
