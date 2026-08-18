using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using GestionDeTurnos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Infrastructure.Repositories
{
    public class UsuarioRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> AddAsync(Usuario user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<Usuario> GetUserAsync(string email)
        { 
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> GetUsuarioByIdAsync(Guid usuarioId)
        {
            if(usuarioId == Guid.Empty)
            {
                return null;
            }
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
        }
    }
}
