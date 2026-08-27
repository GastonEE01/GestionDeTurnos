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
    public class GetLocalUseCase
    {
        private readonly ILocalRepository _localRepository;
        private readonly IMapper _mapper;

            public GetLocalUseCase(ILocalRepository localRepository, IMapper mapper)
        {
            _localRepository = localRepository;
            _mapper = mapper;
         }
        public async Task<List<AddLocalResponseDto>> GetLocal()
        {
            var locales = await _localRepository.GetAll();

            // Mapear la lista completa
            var  localDto = _mapper.Map<List<AddLocalResponseDto>>(locales);
            return localDto;
        }

       

       
    }
}
