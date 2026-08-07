using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Servicios
{
    public class UpdateServiceUseCase
    {
        private readonly IServicioRepository _servicioRepository;

        public UpdateServiceUseCase(IServicioRepository servicioRepository)
        {
            _servicioRepository = servicioRepository;
        }

        public async Task<UpdateServiceResponseDto> UpdateService(Guid servicioId, UpdateServiceRequestDto dto)
        {
            Servicio searchService = await _servicioRepository.GetServiceById(servicioId);

            if (searchService == null) throw new KeyNotFoundException("No se encontro el service");

            // Actualizar los campos del servicio
            if(!string.IsNullOrEmpty(dto.Name)) searchService.Name = dto.Name;
            if(!string.IsNullOrEmpty(dto.Description)) searchService.Description = dto.Description;
            if(dto.DurationInMinutes.HasValue) searchService.DurationInMinutes = dto.DurationInMinutes.Value;
            if(dto.Price.HasValue) searchService.Price = dto.Price.Value;

             await _servicioRepository.UpdateService(searchService);
            // Mapear el response
            UpdateServiceResponseDto response = new UpdateServiceResponseDto
            {
                Name = searchService.Name,
                Description = searchService.Description,
                DurationInMinutes = searchService.DurationInMinutes,
                Price = searchService.Price
            };
            return response;
        }
    }
}
