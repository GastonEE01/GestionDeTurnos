using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.Interface
{
    public interface IUserRepository
    {
        Task<Usuario> Add(Usuario user);
        Task<bool> ExistsByEmailAsync(string email);
        Task<Usuario?> GetUserAsync(string email);
        Task<Usuario?> GetUsuarioByIdAsync(Guid usuarioId);
    }
}
