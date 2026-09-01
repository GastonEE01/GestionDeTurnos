using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Servicios
{
    public class DeleteServiceUseCase
    {
        private readonly IServicioRepository _servicioRepository;

        public DeleteServiceUseCase(IServicioRepository servicioRepository)
        {
            _servicioRepository = servicioRepository;
        }

        public async Task DeleteService(Guid serviceId)
        {
            Servicio searchService = await _servicioRepository.GetServiceById(serviceId);
            if (searchService == null) throw new ArgumentException("No se encontro el servicio para eliminar");
   
            await _servicioRepository.Delete(searchService);
        }
    }
}
