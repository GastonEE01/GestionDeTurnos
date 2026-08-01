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
    public class UsuarioRepository : IUserReposirtory
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public Usuario Add(Usuario user)
        {
            _context.Add(user);
            _context.SaveChanges();
            return user;
        }

        public Usuario GetUser(string email)
        { 
            return _context.Usuarios.FirstOrDefault(l => l.Email == email);
                
        }
    }
}
