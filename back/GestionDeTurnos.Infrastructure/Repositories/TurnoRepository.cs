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

        /*public bool AppointmentExists(Guid localId, Guid servicioId, DateTime date)
        {
            throw new NotImplementedException();
        }*/
    }
}
