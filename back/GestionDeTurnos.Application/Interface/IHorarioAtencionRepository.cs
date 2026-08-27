using GestionDeTurnos.Application.DTOs.HorarioAtencion;
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
        Task Add(HorarioAtencion horarios);
        Task <HorarioAtencion> existingHorarios(List<HorarioAtencionRequestDto> horarios);
        Task<List<HorarioAtencion>> GetHorarioByLocalId(Guid localId);
        Task SaveChangesAsync();
        Task<HorarioAtencion> UpdateHorario(HorarioAtencion existingHorario);
    }
}
