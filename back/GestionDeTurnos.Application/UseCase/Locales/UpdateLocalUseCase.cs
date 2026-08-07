using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Locales
{
    public class UpdateLocalUseCase
    {
        private readonly ILocalRepository _localRepository;

        public UpdateLocalUseCase(ILocalRepository localRepository)
        {
            _localRepository = localRepository;
        }

        public async Task<UpdateLocalResponseDto> ExecuteAsync(Guid idLocal, UpdateLocalRequestDto dto)
        {
            Local? local = await _localRepository.GetLocalById(idLocal);
            if (local == null)
            {
                throw new KeyNotFoundException("Local no encontrado");
            }

            // Verificar si los campos del DTO son nulos o vacíos antes de actualizar
            if (!string.IsNullOrEmpty(dto.Name)) local.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Description)) local.Description = dto.Description;
            if (!string.IsNullOrEmpty(dto.Category)) local.Category = dto.Category;
            if (!string.IsNullOrEmpty(dto.ImageURL)) local.ImageURL = dto.ImageURL;  
            if (!string.IsNullOrEmpty(dto.Title)) local.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Direction)) local.Direction = dto.Direction;
            if (!string.IsNullOrEmpty(dto.Phone)) local.Phone = dto.Phone;

            await _localRepository.UpdateAsync(local);
            // Mapear el response 
            UpdateLocalResponseDto responseDto = new UpdateLocalResponseDto
            {   
                Name = local.Name,
                Description = local.Description,
                Category = local.Category,
                ImageURL = local.ImageURL,
                Title = local.Title,
                Direction = local.Direction,
                Phone = local.Phone
            };
            return responseDto;
        }

       
    }
}
