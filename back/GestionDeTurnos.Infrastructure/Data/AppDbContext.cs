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

            /*
             Lo que plantea esa tarea ("Crear un índice único compuesto") es una capa de seguridad extra. Es como poner un candado con llave en la base de datos para que, si por algún error del frontend o del backend se intenta guardar dos veces el mismo horario para el mismo local en el mismo día, la base de datos lo rebote y diga: "Pará, esto ya existe".
             Para que te quede el panorama claro para mañana, en tu caso actual no sería con (IdLocal + Fecha + Hora), porque nosotros no estamos usando Fecha, sino el DiaSemana (Lunes, Martes, etc.).
             Entonces, para tu proyecto, la regla del candado sería: Un local no puede tener dos horarios de apertura repetidos el mismo día de la semana. (IdLocal + DiaSemana + HoraApertura).
            */
            modelBuilder.Entity<HorarioAtencion>()
                .HasIndex(h => new { h.LocalId, h.DiaSemana, h.HoraApertura })
                .IsUnique();

            modelBuilder.Entity<Turno>()
                .HasIndex(t => new { t.LocalId, t.Date })
                .IsUnique();
        }

       
    }
}
