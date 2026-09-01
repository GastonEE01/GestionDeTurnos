using GestionDeTurnos.Application.DTOs.HorarioAtencion;
using GestionDeTurnos.Application.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs.Local
{
    public class RegisterLocalRequest
    {
        public UserRequestDto User { get; set; }
        public LocalRequestDto Local { get; set; }
        public List<HorarioAtencionRequestDto> Horarios { get; set; }
    }
}
