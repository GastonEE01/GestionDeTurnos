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
    public class GetLocalUseCase
    {
        private readonly ILocalRepository _localRepository;
        private readonly IMapper _mapper;

            public GetLocalUseCase(ILocalRepository localRepository, IMapper mapper)
        {
            _localRepository = localRepository;
            _mapper = mapper;
         }
        public List<LocalDto> GetLocal()
        {
            List<Local> locales = _localRepository.GetAll();

            // Mapear la lista completa
            List<LocalDto> localDto = _mapper.Map<List<LocalDto>>(locales);
            return localDto;
        }
    }
}
