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

        public void Add(Servicio servicio)
        {
            throw new NotImplementedException();
        }

        public async Task<Servicio> AddServicio(Servicio servicio)
        {
            try
            {
                _context.Servicios.Add(servicio);
                await _context.SaveChangesAsync();
                return servicio;
            }
            catch (Exception ex)
            {
                var error = ex.InnerException.Message;
                Console.WriteLine($"🚨 ERROR REAL DE POSTGRES: {error}");
                throw;
            }
        }

        public void DeleteServicio(Servicio searchSservice)
        {
            _context.Servicios.Remove(searchSservice);
            _context.SaveChanges();
        }

        public Task<Servicio> GetServiceById(Guid serviceId)
        {
            return _context.Servicios
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

        public Task UpdateService(Servicio searchService)
        {
            _context.Servicios.Update(searchService);
            return _context.SaveChangesAsync();
        }
    }
}

