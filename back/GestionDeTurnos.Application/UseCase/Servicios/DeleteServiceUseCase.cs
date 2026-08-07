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
            Servicio searchSservice = await _servicioRepository.GetServiceById(serviceId);
            if (searchSservice == null)
            {
                throw new ArgumentException("No se encontro el servicio para eliminar");

            }
            _servicioRepository.DeleteServicio(searchSservice);
        }
    }
}
