using GestionDeTurnos.Application.DTOs.HorarioAtencion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs.Local
{
    public class UpdateLocalRequestDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ImageURL { get; set; }
        public string? Direction { get; set; }
        public string? Phone { get; set; }
        public List<HorarioAtencionRequestDto>? Horarios { get; set; }
    }
}
