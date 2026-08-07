using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs
{
    public class ServicioResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
        public decimal Price { get; set; }
        public Guid LocalId { get; set; }
    }
}
