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
    public class LocalRepository : ILocalRepository
    {
        private readonly AppDbContext _context;

        public LocalRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Local local)
        {
            _context.Locales.Add(local);
            _context.SaveChanges();
        }

        public List<Local> GetAll()
        {
            return _context.Locales
                   .Include(l => l.Servicios) 
                   .Include(l => l.HorariosAtencion)
                   .ToList();

        }
    }
}
