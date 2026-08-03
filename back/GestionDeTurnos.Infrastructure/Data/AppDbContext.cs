using GestionDeTurnos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        }

        public DbSet<Local> Locales { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<HorarioAtencion> HorariosAtencion { get; set; }


        // Para configurar la relación entre Local y HorarioAtencion, y para asegurarnos de que las horas se guarden correctamente en la base de datos.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🕒 Le indicamos a la base de datos que guarde las horas como tipo 'time'
            modelBuilder.Entity<HorarioAtencion>()
                .Property(h => h.HoraApertura)
                .HasColumnType("time");

            modelBuilder.Entity<HorarioAtencion>()
                .Property(h => h.HoraCierre)
                .HasColumnType("time");

            // 💥 Configuración de Borrado en Cascada para los Horarios
            modelBuilder.Entity<Local>()
                .HasMany(l => l.HorariosAtencion)
                .WithOne(h => h.Local)
                .HasForeignKey(h => h.LocalId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
