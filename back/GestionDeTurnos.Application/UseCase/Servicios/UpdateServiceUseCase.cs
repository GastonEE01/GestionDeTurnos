using AutoMapper;
using GestionDeTurnos.Application.DTOs.Servicio;
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
        private readonly IMapper _mapper;


        public UpdateServiceUseCase(IServicioRepository servicioRepository,IMapper mapper)
        {
            _servicioRepository = servicioRepository;
            _mapper = mapper;
        }

        public async Task<UpdateServicioResponseDto> UpdateService(Guid localId,Guid servicioId, UpdateServicioRequestDto dto)
        {
            Servicio searchService = await _servicioRepository.GetServiceById(servicioId);

            if (searchService.LocalId != localId) throw new InvalidOperationException("El servicio no pertenece al local especifico");
           
            if (searchService == null) throw new KeyNotFoundException("No se encontro el service");

            if (dto == null) throw new ArgumentException("Debe proporcionar al menos un campo actualizar.");

            if (!string.IsNullOrEmpty(dto.Name) && dto.Name != searchService.Name)
            {
                bool existeNombre = await _servicioRepository.ExistsByNameAsync(dto.Name);
                if (existeNombre)
                    throw new InvalidOperationException("Ya existe otro servicio con ese nombre.");
            }

            // Actualizar los campos del servicio
            if (!string.IsNullOrEmpty(dto.Name)) searchService.Name = dto.Name;
            if(!string.IsNullOrEmpty(dto.Description)) searchService.Description = dto.Description;
            if(dto.DurationInMinutes.HasValue) searchService.DurationInMinutes = dto.DurationInMinutes.Value;
            if(dto.Price.HasValue) searchService.Price = dto.Price.Value;

             await _servicioRepository.Update(searchService);

            UpdateServicioResponseDto response = _mapper.Map<UpdateServicioResponseDto>(searchService);
            response.Message = "Servicion actualizado.";
            return response;
          
        }
    }
}
