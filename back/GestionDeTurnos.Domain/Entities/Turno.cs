using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Domain.Entities
{
    public class Turno
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } // esto ya no

        // 👤 Quién saca el turno (Si aún no tenés tabla de usuarios, podés dejar ClienteName por ahora)
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; } // Su respectiva propiedad de navegación

        // 💈 Qué servicio se va a realizar
        public Guid ServicioId { get; set; }
        public Servicio? Servicio { get; set; } // Propiedad de navegación

        // 📍 En qué local se realiza el turno
        public Guid LocalId { get; set; }
        public Local? Local { get; set; } // Propiedad de navegación

        // Agrego si el turno ya esta pedido o no
        public bool EstaPedido { get; set; } = true; // Por defecto, cuando se crea un turno, está pedido
    }
}
