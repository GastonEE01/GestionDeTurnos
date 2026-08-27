using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.Interface
{
    public interface IServicioRepository
    {
        Task<Servicio> Add(Servicio servicio);
        Task Delete(Servicio searchSservice);
        Task<bool> ExistsByNameAsync(string name);
        Task<Servicio> GetServiceById(Guid serviceId);
        Task<List<Servicio>> GetServiciosByLocal(Guid localId);
        Task<bool> ServiceExistsInLocalAsync(string name, Guid localId);
        Task<Servicio> Update(Servicio searchService);
    }
}
