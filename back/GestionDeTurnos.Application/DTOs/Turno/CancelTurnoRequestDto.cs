using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs.Turno
{
    public class CancelTurnoRequestDto
    {
        public Guid UsuarioId { get; set; } 
        public Guid TurnoId { get; set; }
    }
}
