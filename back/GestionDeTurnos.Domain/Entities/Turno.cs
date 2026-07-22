using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Domain.Entities
{
    public class Turno
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } // Guarda fecha y hora del turno

        // 👤 Quién saca el turno (Si aún no tenés tabla de usuarios, podés dejar ClienteName por ahora)
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; } // Su respectiva propiedad de navegación

        // 💈 Qué servicio se va a realizar
        public int ServicioId { get; set; }
        public Servicio? Servicio { get; set; } // Propiedad de navegación

        // 📍 En qué local se realiza el turno
        public int LocalId { get; set; }
        public Local? Local { get; set; } // Propiedad de navegación
    }
}
