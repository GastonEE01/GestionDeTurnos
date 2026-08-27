using GestionDeTurnos.Application.DTOs.Local;
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
            if (local == null) throw new KeyNotFoundException("Local no encontrado");

            if (dto == null) throw new ArgumentException("Debe proporcionar al menos un campo para actualizar.");

            if (!string.IsNullOrEmpty(dto.Name) && dto.Name != local.Name)
            {
                bool existeNombre = await _localRepository.ExistsByNameAsync(dto.Name);
                if (existeNombre)
                    throw new InvalidOperationException("Ya existe otro local con ese nombre.");
            }

            // Verificar si los campos del DTO son nulos o vacíos antes de actualizar
            if (!string.IsNullOrEmpty(dto.Name)) local.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Description)) local.Description = dto.Description;
            if (!string.IsNullOrEmpty(dto.Category)) local.Category = dto.Category;
            if (!string.IsNullOrEmpty(dto.ImageURL)) local.ImageURL = dto.ImageURL;  
            if (!string.IsNullOrEmpty(dto.Direction)) local.Direction = dto.Direction;
            if (!string.IsNullOrEmpty(dto.Phone)) local.Phone = dto.Phone;

            await _localRepository.Update(local);
            UpdateLocalResponseDto responseDto = new UpdateLocalResponseDto
            {   
                Name = local.Name,
                Description = local.Description,
                Category = local.Category,
                ImageURL = local.ImageURL,
                Direction = local.Direction,
                Phone = local.Phone,
                Message = "Local actualizado"
            };
            return responseDto;
        }

       
    }
}
