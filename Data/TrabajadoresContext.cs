using Microsoft.EntityFrameworkCore;
using Myper.Trabajadores.Web.Models;

namespace Myper.Trabajadores.Web.Data
{
    // DbContext principal de la aplicación
    public class TrabajadoresContext : DbContext
    {
        public TrabajadoresContext(DbContextOptions<TrabajadoresContext> options)
            : base(options)
        {
        }

        public DbSet<Trabajador> Trabajadores { get; set; }

        // Configuración de mapeo entre la entidad y la tabla SQL
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Trabajador>(entity =>
            {
                entity.ToTable("Trabajadores");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombres)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Apellidos)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.TipoDocumento)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(e => e.NumeroDocumento)
                      .IsRequired()
                      .HasMaxLength(20);

                // Evita números de documento duplicados
                entity.HasIndex(e => e.NumeroDocumento)
                      .IsUnique();

                entity.Property(e => e.Sexo)
                      .IsRequired()
                      .HasColumnType("char(1)");

                entity.Property(e => e.FechaNacimiento)
                      .HasColumnType("date");

                entity.Property(e => e.Foto)
                      .HasColumnType("nvarchar(max)");

                entity.Property(e => e.Direccion)
                      .HasMaxLength(200);

                entity.Property(e => e.FechaRegistro)
                      .HasColumnType("datetime");

                entity.Property(e => e.Activo)
                      .HasColumnType("bit");
            });
        }
    }
}