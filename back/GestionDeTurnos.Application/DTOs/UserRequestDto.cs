using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs
{
    public class UserRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password{ get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
       
    }
}
