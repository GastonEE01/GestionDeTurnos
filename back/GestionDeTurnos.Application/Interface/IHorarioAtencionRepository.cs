using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.Interface
{
    public interface IHorarioAtencionRepository
    {
        void Add(HorarioAtencion horarios);
        HorarioAtencion existingHorarios(List<HorarioAtencionRequestDto> horarios);
        Task<List<HorarioAtencion>> GetHorarioByLocalId(Guid localId);
        Task SaveChangesAsync();
        Task<HorarioAtencion> UpdateHorario(HorarioAtencion existingHorario);
    }
}
