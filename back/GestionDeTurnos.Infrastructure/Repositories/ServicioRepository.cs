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
    public class ServicioRepository : IServicioRepository
    {
        private readonly AppDbContext _context;

        public ServicioRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Servicio> Add(Servicio servicio)
        {
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
            return servicio;
        }

        public async Task Delete(Servicio searchSservice)
        {
            _context.Servicios.Remove(searchSservice);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Servicios
                    .AnyAsync(l => l.Name.ToLower() == name.ToLower());
        }

        public async Task<Servicio> GetServiceById(Guid serviceId)
        {
            return await _context.Servicios
                .Include(s => s.Local)
                .FirstOrDefaultAsync(s => s.Id == serviceId);
        }

        public async Task<List<Servicio>> GetServiciosByLocal(Guid localId)
        {
            return await _context.Servicios
                .Include(s => s.Local)
                .Where(s => s.LocalId == localId)
                .ToListAsync();
        }

        public async Task<bool> ServiceExistsInLocalAsync(string name, Guid localId)
        {
            return await _context.Servicios
                .AnyAsync(s => s.LocalId == localId && s.Name.ToLower() == name.ToLower());
        }

        public async Task<Servicio> Update(Servicio searchService)
        {
            _context.Servicios.Update(searchService);
            await _context.SaveChangesAsync();
            return searchService;
        }
    }
}

