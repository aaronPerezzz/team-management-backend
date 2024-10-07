using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using team_management_backend.Entities;

namespace team_management_backend
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Asignacion> Asignaciones => Set<Asignacion>();
        public DbSet<CaracteristicasTransporte> CaracteristicasTransportes => Set<CaracteristicasTransporte>();
        public DbSet<Equipo> Equipos => Set<Equipo>();
        public DbSet<EquipoHardware> EquiposHardware => Set<EquipoHardware>();
        public DbSet<EquipoSoftware> EquiposSoftware => Set<EquipoSoftware>();
        public DbSet<Garantia> Garantias => Set<Garantia>();
        public DbSet<Hardware> Hardwares => Set<Hardware>();
        public DbSet<Poliza> Polizas => Set<Poliza>();
        public DbSet<Software> Softwares => Set<Software>();
        public DbSet<TipoAsignacion> TiposAsignacion => Set<TipoAsignacion>();
        public DbSet<TipoEquipo> TiposEquipo => Set<TipoEquipo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Equipo)            
                .WithOne()                         
                .HasForeignKey<Asignacion>(a => a.IdEquipo);

            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.TipoAsignacion)  
                .WithMany(t => t.Asignaciones)    
                .HasForeignKey(a => a.IdTipoAsignacion);


            modelBuilder.Entity<CaracteristicasTransporte>()
                .HasOne(ct => ct.Equipo)
                .WithOne(e => e.CaracteristicasTransporte)
                .HasForeignKey<CaracteristicasTransporte>(ct => ct.Id);             


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

            modelBuilder.Entity<TipoAsignacion>()
                .HasMany(ta => ta.Asignaciones)
                .WithOne(a => a.TipoAsignacion)
                .HasForeignKey(a => a.IdTipoAsignacion);

            modelBuilder.Entity<TipoEquipo>()
                .HasMany(te => te.Equipos)
                .WithOne(e => e.TipoEquipo)
                .HasForeignKey(e => e.IdTipoEquipo);
        }
    }

}
