using AutoMapper;
using GestionDeTurnos.Application.DTOs.HorarioAtencion;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Horarios
{
    public class GetHorariosByLocalUseCase
    {
        private readonly IHorarioAtencionRepository _horarioAtencionRepository;
        private readonly IMapper _mapper;

        public GetHorariosByLocalUseCase(IHorarioAtencionRepository horarioAtencionRepository, IMapper mapper)
        {
            _horarioAtencionRepository = horarioAtencionRepository;
            _mapper = mapper;
        }

        public async Task<List<HorarioAtencionResponseDto>> GetHorariosAtencionByLocal(Guid localId)
        {
            List<HorarioAtencion> searchHorarios = await _horarioAtencionRepository.GetHorarioByLocalId(localId);
            if(searchHorarios == null || !searchHorarios.Any()) throw new KeyNotFoundException("No se encontraron horarios para el local especificado.");

            List<HorarioAtencionResponseDto> response = _mapper.Map<List<HorarioAtencionResponseDto>>(searchHorarios);
            return response;
        }
    }
}
