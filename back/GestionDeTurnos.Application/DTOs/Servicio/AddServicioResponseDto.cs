using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs.Servicio
{
    public class AddServicioResponseDto
    {
        public Guid UsuarioId { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
        public decimal Price { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
