using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.Interface
{
    public interface ILocalRepository
    {
        void Add(Local local);
        Task <List<Local>> GetAll();
        Task <List<Local>> GetLocalByUser(Guid userId);
        HorarioAtencion GetHorarioByLocalId(Guid localId);
        Task<Local?> GetLocalById(Guid localId);
        Servicio GetServicioById(Guid servicioId);
        Task<bool> IsLocalOwnerAsync(Guid localId, Guid usuarioId);
        void DeleteLocal(Local searchLocal);
        Task UpdateAsync(Local local);
        //Local GetById(Guid id);
    }
}
