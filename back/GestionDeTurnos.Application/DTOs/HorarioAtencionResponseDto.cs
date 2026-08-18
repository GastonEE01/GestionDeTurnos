using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs
{
    public class HorarioAtencionResponseDto
    {
        public Guid Id { get; set; }
        public DayOfWeek DiaSemana { get; set; }

        // 🕒 Usamos TimeSpan para guardar solo la hora del día (sin fecha)
        public TimeSpan HoraApertura { get; set; } // Ejemplo: 08:00:00
        public TimeSpan HoraCierre { get; set; }   // Ejemplo: 17:00:00

        public bool EstaCerrado { get; set; }
        public Guid LocalId { get; set; }
    }
}
