using AutoMapper;
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
    public  class GetLocalesByUsuarioIdUseCase
    {
        private readonly ILocalRepository _localRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetLocalesByUsuarioIdUseCase(ILocalRepository localRepository, IUserRepository userRepository,IMapper mapper)
        {
            _localRepository = localRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<AddLocalResponseDto>> GetLocalesByUsuario(Guid usuarioId)
        {
            Usuario searchUser = await _userRepository.GetUsuarioByIdAsync(usuarioId);
            if (searchUser == null) throw new Exception("No se encontró el usuario.");

            List<Local> locales = await _localRepository.GetLocalByUser(usuarioId);

            List<AddLocalResponseDto> localDto = _mapper.Map<List<AddLocalResponseDto>>(locales);
            return localDto;
           
        }
    }
}
