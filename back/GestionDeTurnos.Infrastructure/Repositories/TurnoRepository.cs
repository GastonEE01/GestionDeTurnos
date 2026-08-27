using GestionDeTurnos.Application.DTOs;
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
    public class TurnoRepository : ITurnoRespository
    {
        private readonly AppDbContext _context;

        public TurnoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AppointmentExistsAsync(Guid localId, DateTime fecha)
        {
            return await _context.Turnos.AnyAsync(t => t.LocalId == localId && t.Date == fecha);
        }

        public async Task<Turno> Add(Turno turno)
        { 
                _context.Add(turno);
                _context.SaveChangesAsync();
                return turno;   
        }

        public async Task Delete(Turno dto)
        {
            _context.Turnos.Remove(dto);
            await _context.SaveChangesAsync();
        }

        public async Task<Turno> GetByTurnoIdAsync(Guid turnoId)
        {
            return await _context.Turnos.FirstOrDefaultAsync(s => s.Id == turnoId);
        }

        public async Task<List<Turno>> GetTurnosByLocalAndFechaAsync(Guid localId, DateTime fecha)
        {
            return await _context.Turnos
         .Where(t => t.LocalId == localId && t.Date.Date == fecha.Date)
         .ToListAsync();

            /*public bool AppointmentExists(Guid localId, Guid servicioId, DateTime date)
            {
                throw new NotImplementedException();
            }*/
        }

        public async Task<List<Turno>> GetTurnosByUsuarioIdAsync(Guid usuarioId)
        {
           return await _context.Turnos
                .Include(t => t.Servicio)
                .Include(t => t.Local)
                .Where(t => t.UsuarioId == usuarioId)
                .OrderByDescending(t => t.Date)
                .ToListAsync() ;
        }
    }
}
