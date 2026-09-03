using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.Interface
{
    public interface ITurnoRespository
    {
        Task<Turno> Add(Turno dto);
        Task Delete(Turno dto);
        Task<bool> AppointmentExistsAsync(Guid localId, DateTime date);
        Task<Turno> GetByTurnoIdAsync(Guid usuarioId);
        Task<List<Turno>> GetTurnosByLocalAndFechaAsync(Guid localId, DateTime fecha);
        Task<List<Turno>> GetTurnosByUsuarioIdAsync(Guid usuarioId);
        Task<List<Turno>> GetTurnosByLocalIdAsync(Guid localId);
    }
}
