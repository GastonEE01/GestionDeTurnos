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
    public class AddServiceUseCase
    {
        private readonly ILocalRepository _localRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServicioRepository _servicioRepository;

        public AddServiceUseCase(ILocalRepository localRepository, IUserRepository userRepository, IServicioRepository servicioRepository)
        {
            _localRepository = localRepository;
            _servicioRepository = servicioRepository;
            _userRepository = userRepository;
        }

        public async Task<Servicio> AddService(AddServiceRequestDto dto, Guid localId)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("No se pudo crear el servico", nameof(dto));
            }
            // Verificar si el usuario tiene permisos para crear un servicio en el local
            Usuario searchUser = await _userRepository.GetUsuarioByIdAsync(dto.UsuarioId);
            if (searchUser == null) { 
                throw new ArgumentException("El usuario no existe", nameof(dto.UsuarioId)); 
            }

            if (searchUser.Rol == "Admin" || searchUser.Rol == "Cliente")
            {
                throw new UnauthorizedAccessException("El usuario no tiene permisos para crear un servicio");
            }

            // Verificar si el usuario tiene un local asociado
            bool userIsOwner = await _localRepository.IsLocalOwnerAsync(localId, dto.UsuarioId);
            if (!userIsOwner)
            {
                throw new InvalidOperationException("El usuario no tiene un local asociado");
            }

            // Validar duplicado de servicio en el mismo local
            bool serviceExists = await _servicioRepository.ServiceExistsInLocalAsync(dto.Name, localId);
            if (serviceExists)
            {
                throw new InvalidOperationException($"Ya existe un servicio llamado '{dto.Name}' en este local.");
            }

            // Crear el servicio
            Servicio servicio = new Servicio
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                DurationInMinutes = dto.DurationInMinutes,
                Price = dto.Price,
                LocalId = localId
            };
            servicio = await _servicioRepository.AddServicio(servicio);
            return servicio;
        }
    }
}
