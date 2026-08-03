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
    public class HorarioAtencionRepository : IHorarioAtencionRepository
    {
        private readonly AppDbContext _context;

        public HorarioAtencionRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(HorarioAtencion horarios)
        {
            _context.HorariosAtencion.Add(horarios);
            _context.SaveChanges();
        }
    }
}
