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
        public DbSet<Garantia> Garantias { get; set; }
        public DbSet<Hardware> Hardwares { get; set; }
        public DbSet<Poliza> Polizas { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<TipoEquipo> TiposEquipo { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

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
                .WithOne(g => g.Equipo)
                .HasForeignKey<Garantia>(g => g.IdEquipo);

            modelBuilder.Entity<Equipo>()
                .HasOne(e => e.Poliza)
                .WithOne(p => p.Equipo)
                .HasForeignKey<Poliza>(p => p.IdEquipo);

            modelBuilder.Entity<Equipo>()
                .HasOne(e => e.CaracteristicasTransporte)
                .WithOne(ct => ct.Equipo)
                .HasForeignKey<CaracteristicasTransporte>(ct => ct.IdEquipo);

            modelBuilder.Entity<Equipo>()
                .HasMany(e => e.Software)
                .WithOne(s => s.Equipo)
                .HasForeignKey(s => s.IdEquipo);

            modelBuilder.Entity<Equipo>()
                .HasMany(e => e.Hardware)    
                .WithOne(h => h.Equipo)
                .HasForeignKey(h => h.IdEquipo);

            modelBuilder.Entity<TipoEquipo>()
                .HasMany(te => te.Equipos)
                .WithOne(e => e.TipoEquipo)
                .HasForeignKey(e => e.IdTipoEquipo);
        }
    }

}
