using GestionDeTurnos.Application.DTOs.HorarioAtencion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs.Local
{
    public class AsociarLocalForUserRequestDto
    {
        public Guid UsuarioId { get; set; }
        public LocalRequestDto Local { get; set; }
        public List<HorarioAtencionRequestDto> Horarios { get; set; }

    }
}
