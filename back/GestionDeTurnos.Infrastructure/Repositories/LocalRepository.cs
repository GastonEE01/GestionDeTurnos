using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using GestionDeTurnos.Infrastructure.Data;
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

        public List<Local> GetAll()
        {
            return _context.Locals.ToList();
          /* List<Local> list = new List<Local>();
            _context.Add(list);
            _context.SaveChanges();
            return list;*/
        }
    }
}
