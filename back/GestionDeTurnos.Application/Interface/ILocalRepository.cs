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
        Task<Local> Add(Local local);
        Task <List<Local>> GetAll();
        Task <List<Local>> GetLocalByUser(Guid userId);

        Task<Local?> GetLocalById(Guid localId);
        Servicio GetServicioById(Guid servicioId);
        Task<bool> IsLocalOwnerAsync(Guid localId, Guid usuarioId);
        Task Delete(Local searchLocal);
        Task<Local> Update(Local local);
        Task<HorarioAtencion> GetHorarioByLocalId(Guid localId);
        Task<bool> ExistsByNameAsync(string name);
        //Local GetById(Guid id);
    }
}
