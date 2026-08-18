using AutoMapper;
using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Horarios
{
    public class UpdateHorariosByLocalUseCase
    {
        private readonly IHorarioAtencionRepository _horarioAtencionRepository;
        private readonly ILocalRepository _localRepository;

        private readonly IMapper _mapper;

        public UpdateHorariosByLocalUseCase(IHorarioAtencionRepository horarioAtencionRepository,ILocalRepository localRepository, IMapper mapper)
        {
            _horarioAtencionRepository = horarioAtencionRepository;
            _mapper = mapper;
            _localRepository = localRepository;
        }

      

        public async Task<List<HorarioAtencionResponseDto>> UpdateHorariosByLocalID(Guid localId,Guid userId, List<HorarioAtencionRequestDto> horarios)
        {
            var local = await _localRepository.GetLocalById(localId);
            if (local == null) throw new Exception("El local especificado no existe");

            if(local.UsuarioId != userId) throw new UnauthorizedAccessException("No tenés permisos para modificar este local.");

            // Buscar el local por su ID
            List<HorarioAtencion> searchHorario = await _horarioAtencionRepository.GetHorarioByLocalId(localId);

            if(searchHorario == null) throw new KeyNotFoundException("No se encontraron horarios para el local especificado.");


            // Actualizar los horarios existentes
            foreach (var horarioDto in horarios)
            {
                HorarioAtencion existingHorario = searchHorario.FirstOrDefault(h => h.DiaSemana == horarioDto.DiaSemana); if (existingHorario != null)

                if (horarioDto.HoraApertura > horarioDto.HoraCierre) throw new ArgumentException("La hora de apertura no puede ser mayor que la de cierre.");
                    
                if (existingHorario != null)
                    {

                        existingHorario.HoraApertura = horarioDto.HoraApertura;
                        existingHorario.HoraCierre = horarioDto.HoraCierre;
                        existingHorario.EstaCerrado = horarioDto.EstaCerrado;
                    }
                    else
                {
                    // Si no existe un horario para ese día, crear uno nuevo
                    var newHorario = new HorarioAtencion
                    {
                        LocalId = localId,
                        DiaSemana = horarioDto.DiaSemana,
                        HoraApertura = horarioDto.HoraApertura,
                        HoraCierre = horarioDto.HoraCierre,
                        EstaCerrado = horarioDto.EstaCerrado
                    };
                    searchHorario.Add(newHorario);
                }
            }
            await _horarioAtencionRepository.SaveChangesAsync();
            return _mapper.Map<List<HorarioAtencionResponseDto>>(searchHorario);
        }
    }
}