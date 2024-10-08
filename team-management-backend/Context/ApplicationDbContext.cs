using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using team_management_backend.domain.Entities;

namespace team_management_backend.Context
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Asignacion> Asignaciones { get; set; }
        public DbSet<CaracteristicasTransporte> CaracteristicasTransportes { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<EquipoHardware> EquiposHardware { get; set; }
        public DbSet<EquipoSoftware> EquiposSoftware { get; set; }
        public DbSet<Garantia> Garantias { get; set; }
        public DbSet<Hardware> Hardwares { get; set; }
        public DbSet<Poliza> Polizas { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<TipoEquipo> TiposEquipo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Equipo)
                .WithMany() 
                .HasForeignKey(a => a.IdEquipo);


            modelBuilder.Entity<Equipo>()
                .HasOne(e => e.TipoEquipo)
                .WithMany(te => te.Equipos)
                .HasForeignKey(e => e.IdTipoEquipo);

            modelBuilder.Entity<Equipo>()
                .HasOne(e => e.Garantia)
                .WithMany()
                .HasForeignKey(e => e.IdGarantia);

            modelBuilder.Entity<Equipo>()
                .HasOne(e => e.Poliza)
                .WithMany()
                .HasForeignKey(e => e.IdPoliza);

            modelBuilder.Entity<Equipo>()
                .HasOne(e => e.CaracteristicasTransporte)
                .WithOne(ct => ct.Equipo)
                .HasForeignKey<CaracteristicasTransporte>(ct => ct.IdEquipo);

            modelBuilder.Entity<EquipoHardware>()
            .HasKey(eh => new { eh.IdEquipo, eh.IdHardware });

            modelBuilder.Entity<EquipoHardware>()
                .HasOne(eh => eh.Equipo)
                .WithMany(e => e.EquiposHardware)
                .HasForeignKey(eh => eh.IdEquipo);

            modelBuilder.Entity<EquipoHardware>()
                .HasOne(eh => eh.Hardware)
                .WithMany(h => h.EquiposHardware)
                .HasForeignKey(eh => eh.IdHardware);

            modelBuilder.Entity<EquipoSoftware>()
            .HasKey(ef => new { ef.IdEquipo, ef.IdSoftware });

            modelBuilder.Entity<EquipoSoftware>()
                .HasOne(es => es.Equipo)
                .WithMany(e => e.EquiposSoftware)
                .HasForeignKey(es => es.IdEquipo);

            modelBuilder.Entity<EquipoSoftware>()
                .HasOne(es => es.Software)
                .WithMany(s => s.EquiposSoftware)
                .HasForeignKey(es => es.IdSoftware);

            modelBuilder.Entity<TipoEquipo>()
                .HasMany(te => te.Equipos)
                .WithOne(e => e.TipoEquipo)
                .HasForeignKey(e => e.IdTipoEquipo);
        }
    }

}
