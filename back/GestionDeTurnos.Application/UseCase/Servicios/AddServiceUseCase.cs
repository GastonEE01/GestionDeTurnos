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
    public class AddServiceUseCase
    {
        private readonly ILocalRepository _localRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServicioRepository _servicioRepository;
        private readonly IMapper _mapper;
        public AddServiceUseCase(ILocalRepository localRepository, IUserRepository userRepository, IServicioRepository servicioRepository,IMapper mapper)
        {
            _localRepository = localRepository;
            _servicioRepository = servicioRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<AddServicioResponseDto> AddService(AddServicioRequestDto dto, Guid localId)
        {
            if (dto == null) throw new ArgumentException("No se pudo crear el servico");

            if (string.IsNullOrEmpty(dto.Name))
                throw new ArgumentException("El nombre del servicio es obligatorio.");

            if (string.IsNullOrEmpty(dto.Description))
                throw new ArgumentException("La descripcion del servicio es obligatorio.");

            if (dto.DurationInMinutes == 0)
                throw new ArgumentException("La duracion del servicio no puede ser cero.");

            if (dto.Price == 0)
                throw new ArgumentException("El precio del servicio no puede ser cero.");

            // Verificar si el usuario tiene permisos para crear un servicio en el local

            Usuario searchUser = await _userRepository.GetUsuarioByIdAsync(dto.UsuarioId);
            if (searchUser == null) throw new KeyNotFoundException("El usuario no existe"); 
            
            if (searchUser.Rol == "Admin" || searchUser.Rol == "Cliente") throw new UnauthorizedAccessException("El usuario no tiene permisos para crear un servicio");
           
            // Verificar si el usuario tiene un local asociado
            bool userIsOwner = await _localRepository.IsLocalOwnerAsync(localId, dto.UsuarioId);
            if (!userIsOwner) throw new InvalidOperationException("El usuario no tiene un local asociado");
         
            // Validar duplicado de servicio en el mismo local
            bool serviceExists = await _servicioRepository.ServiceExistsInLocalAsync(dto.Name, localId);
            if (serviceExists) throw new InvalidOperationException($"Ya existe un servicio llamado '{dto.Name}' en este local.");
         
            Servicio servicio = new Servicio
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                DurationInMinutes = dto.DurationInMinutes,
                Price = dto.Price,
                LocalId = localId
            };

            await _servicioRepository.Add(servicio);

            var responseDto = _mapper.Map<AddServicioResponseDto>(servicio);
            responseDto.UsuarioId = dto.UsuarioId;
            responseDto.Message = "Servicio creado exitosamente.";

            return responseDto;
        }
    }
}
