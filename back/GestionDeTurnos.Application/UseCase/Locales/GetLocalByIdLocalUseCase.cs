using AutoMapper;
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
    public class GetLocalByIdLocalUseCase
    {
        private readonly ILocalRepository _localRepository;
        private readonly IMapper _mapper;

        public GetLocalByIdLocalUseCase(ILocalRepository localRepository, IMapper mapper)
        {
            _localRepository = localRepository;
            _mapper = mapper;
        }

        public async Task<LocalResponseDto> GetLocalById(Guid id)
        {
            Local searchLocal = await _localRepository.GetLocalById(id);
            if (searchLocal == null)
            {
                throw new Exception("No se encontró el local.");
            }
            return _mapper.Map<LocalResponseDto>(searchLocal);
        }
    }
}
