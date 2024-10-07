﻿using System.ComponentModel.DataAnnotations;

namespace team_management_backend.Entities
{
    public class Asignacion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IdEquipo { get; set; }
        [Required]
        public int IdTipoAsignacion { get; set; }
        [Required]
        public string IdUsuario { get; set; } = null!;
        public DateTime FechaAsignacion { get; set; }
        public DateTime FechaFinAsignacion { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        // Relaciones
        public Equipo Equipo { get; set; }
        public TipoAsignacion TipoAsignacion { get; set; }
    }

}
