using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs
{
    public class LoginResponseDto
    {
        public Guid Id { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}
