using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs.TurnoDTO
{
    public class TurnoDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid LocalId { get; set; }
        public Guid ServicioId { get; set; }
        public bool EstaPedido { get; set; }
        public string ServicioName { get; set; }  // <-- Agregado
        public string LocalName { get; set; }     // <-- Agregado
    }
}
