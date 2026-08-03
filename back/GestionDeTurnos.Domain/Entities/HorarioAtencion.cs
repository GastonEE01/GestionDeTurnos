using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Domain.Entities
{
    public class HorarioAtencion
    {
        public Guid Id { get; set; }

        // 📍 Relación con el Local
        public Guid LocalId { get; set; }
        public Local? Local { get; set; }

        // 📅 Día de la semana (0 = Domingo, 1 = Lunes, ..., 6 = Sábado)
        // Usar DayOfWeek de C# te va a facilitar la vida para comparar fechas después
        public DayOfWeek DiaSemana { get; set; }

        // 🕒 Usamos TimeSpan para guardar solo la hora del día (sin fecha)
        public TimeSpan HoraApertura { get; set; } // Ejemplo: 08:00:00
        public TimeSpan HoraCierre { get; set; }   // Ejemplo: 17:00:00

        public bool EstaCerrado { get; set; } // Por si un local decide cerrar un día específico de la semana
    }
}
