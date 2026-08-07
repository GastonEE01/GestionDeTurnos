using AutoMapper;
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
    public class GetServiceByLocal
    {
        private readonly IServicioRepository _servicioRepository;
        private readonly ILocalRepository _localRepository;
        private readonly IMapper _mapper;

        public GetServiceByLocal(IServicioRepository servicioRepository, ILocalRepository localRepository, IMapper mapper)
        {
            _servicioRepository = servicioRepository;
            _localRepository = localRepository;
            _mapper = mapper;
        }

        public async Task<List<ServicioResponseDto>> GetServiciosByLocal(Guid localId)
        {
            Local searchLocal = await _localRepository.GetLocalById(localId);

            if (searchLocal == null) throw new Exception("No se encontro el local");

            List<Servicio> servicios = await _servicioRepository.GetServiciosByLocal(localId);
            List<ServicioResponseDto> servicioDto = _mapper.Map<List<ServicioResponseDto>>(servicios);
            return servicioDto;
        }
    }
}


