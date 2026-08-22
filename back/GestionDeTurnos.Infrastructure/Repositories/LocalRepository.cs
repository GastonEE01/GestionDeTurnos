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

       

        public async Task<List<Local>> GetAll()
        {
            return await _context.Locales
                   .Include(l => l.Servicios) 
                   .Include(l => l.HorariosAtencion)
                   .ToListAsync();

        }

        public async Task<Local?> GetById(Guid id)
        {
            return await _context.Locales
                .Include(l => l.Servicios)
                .Include(l => l.HorariosAtencion)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task DeleteLocal(Local searchLocal)
        {
            _context.Locales.Remove(searchLocal);
            await _context.SaveChangesAsync();
        }

        public async Task<HorarioAtencion> GetHorarioByLocalId(Guid localId)
        {
            return await _context.HorariosAtencion.FirstOrDefaultAsync(h => h.LocalId == localId);
        }

        public async Task<Local?> GetLocalById(Guid localId)
        {
            return await _context.Locales
                .Include(l => l.HorariosAtencion)
                .FirstOrDefaultAsync(l => l.Id == localId);
        }

        public async Task<List<Local>> GetLocalByUser(Guid userId)
        {
            return await _context.Locales
                .Include(l => l.HorariosAtencion)
                .Include(l => l.Servicios)
                .Where(l => l.UsuarioId == userId)
                .ToListAsync();
        }

        public Servicio GetServicioById(Guid servicioId)
        {
            return _context.Servicios.FirstOrDefault(s => s.Id == servicioId);
        }

        public async Task<bool> IsLocalOwnerAsync(Guid localId, Guid usuarioId)
        {
            return await _context.Locales
                .AnyAsync(l => l.Id == localId && l.UsuarioId == usuarioId);
        }

        public Task UpdateAsync(Local local)
        {
            _context.Locales.Update(local);
            return _context.SaveChangesAsync();
        }

       
    }
}
