using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs.Local
{
    public class LocalRequestDto
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
      //  public string Title { get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
