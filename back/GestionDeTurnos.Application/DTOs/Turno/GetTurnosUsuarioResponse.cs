using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs.TurnoDTO
{
    public class GetTurnosUsuarioResponse
    {
        public Guid UsuarioId { get; set; }
        public string NameUser { get; set; } = string.Empty;
        public List<TurnoDto> Turnos { get; set; } = new();
    }
}
