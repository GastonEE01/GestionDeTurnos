using GestionDeTurnos.Application.DTOs.HorarioAtencion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs.Local
{
    public class AddLocalRequest
    {
        public LocalRequestDto Local { get; set; } = new LocalRequestDto();
        public List<HorarioAtencionRequestDto> Horarios { get; set; } = new List<HorarioAtencionRequestDto>();

    }
}
