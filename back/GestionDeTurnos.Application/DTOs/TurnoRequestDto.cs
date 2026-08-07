using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs
{
    public class TurnoRequestDto
    {
        //public DateTime Date { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public Guid ServicioId { get; set; } 
        public Guid LocalId { get; set; }
        public Guid UsuarioId { get; set; }
        public bool EstaPedido { get; set; } = true; // Por defecto, cuando se crea un turno, está pedido


        // Guarda fecha y hora del turno

    }
}
