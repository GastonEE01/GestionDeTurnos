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

        public async Task<Turno> addTurnoAsync(Turno turno)
        {
            try
            {

                _context.Add(turno);
                _context.SaveChanges();
                return turno;
            }
            catch (DbUpdateException ex)
            {
                var error = ex.InnerException.Message;
                Console.WriteLine($"🚨 ERROR REAL DE POSTGRES: {error}");
                throw;
            }
        }

        public async Task DeleteTurno(Turno dto)
        {
            _context.Turnos.Remove(dto);
            _context.SaveChangesAsync();
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
    }
}
