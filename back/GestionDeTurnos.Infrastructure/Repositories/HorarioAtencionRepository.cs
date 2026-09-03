using GestionDeTurnos.Application.DTOs.HorarioAtencion;
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
    public class HorarioAtencionRepository : IHorarioAtencionRepository
    {
        private readonly AppDbContext _context;

        public HorarioAtencionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(HorarioAtencion horario)
        {
            _context.HorariosAtencion.AddAsync(horario);
        }

        public async Task Delete(HorarioAtencion hExistente)
        {
            _context.HorariosAtencion.Remove(hExistente);
        }

        public async Task<HorarioAtencion?> existingHorarios(List<HorarioAtencionRequestDto> horarios)
        {
            if (horarios == null || !horarios.Any()) return null;

            return await _context.HorariosAtencion.FirstOrDefaultAsync(h => h.DiaSemana == horarios[0].DiaSemana && h.LocalId == horarios[0].HorarioId);
        }

        public Task<List<HorarioAtencion>> GetHorarioByLocalId(Guid localId)
        {
            return _context.HorariosAtencion
                   .Include(h => h.Local)
                   .Where(h => h.LocalId == localId)
                   .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<HorarioAtencion> UpdateHorario(HorarioAtencion existingHorario)
        {
            _context.HorariosAtencion.Update(existingHorario);
            await _context.SaveChangesAsync();
            return existingHorario;
        }

      
    }
}
