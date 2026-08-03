using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs
{
    public class CrearLocalRequest
    {
        public LocalRequestDto Local { get; set; } = new LocalRequestDto();
        //public HorarioAtencionRequestDto Horario { get; set; } = new HorarioAtencionRequestDto();
        public List<HorarioAtencionRequestDto> Horarios { get; set; } = new List<HorarioAtencionRequestDto>();

    }
}
