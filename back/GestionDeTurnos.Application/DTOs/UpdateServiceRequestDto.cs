using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs
{
    public class UpdateServiceRequestDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; } 
        public int? DurationInMinutes { get; set; }
        public decimal? Price { get; set; }
    }
}
