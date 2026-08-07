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
        Task<Usuario> AddAsync(Usuario user);
        Task<Usuario?> GetUserAsync(string email);
        Task<Usuario?> GetUsuarioByIdAsync(Guid usuarioId);
    }
}
