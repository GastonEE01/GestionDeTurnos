using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs.Turno
{
    public class AddTurnoResponseDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Message  { get; set; } = string.Empty;
        public bool EstaPedido { get; set; } = true; 
        public Guid LocalId { get; set; }
        public Guid ServicioId { get; set; }
        public Guid UsuarioId { get; set; }

    }
}
