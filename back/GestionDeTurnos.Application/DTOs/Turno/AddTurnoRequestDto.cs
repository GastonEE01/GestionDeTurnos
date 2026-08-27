using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs.Turno
{
    public class AddTurnoRequestDto
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public Guid ServicioId { get; set; } 
        public Guid LocalId { get; set; }
        public Guid UsuarioId { get; set; }
        public bool EstaPedido { get; set; } = true; 
    }
}
