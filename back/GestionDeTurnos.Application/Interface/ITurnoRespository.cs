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
        Task<Turno> addTurnoAsync(Turno dto);
        Task<bool> AppointmentExistsAsync(Guid localId, DateTime date);
    }
}
