using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Domain.Entities
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }  
        public decimal Price { get; set; }

        // Relación con el Local
        public int LocalId { get; set; } 
        public Local Local { get; set; }  = null!;
    }
}
