using AutoMapper;
using GestionDeTurnos.Application.DTOs.Turno;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Turnos
{
    public class AddTurnoUseCase
    {
        private readonly ITurnoRespository _turnoRespository;
        private readonly IUserRepository _usuarioRepository;
        private readonly ILocalRepository _localRepository;
        private readonly IServicioRepository _servicioRepository;

        private readonly IMapper _mapper;
        public AddTurnoUseCase(ITurnoRespository turnoRespository, IUserRepository usuarioRepository, ILocalRepository localRepository,IServicioRepository servicioRepository, IMapper mapper)
        {
            _turnoRespository = turnoRespository;
            _usuarioRepository = usuarioRepository;
            _localRepository = localRepository;
            _servicioRepository = servicioRepository;
            _mapper = mapper;
        }
    

    public async Task<AddTurnoResponseDto> AddTurno(AddTurnoRequestDto dto)
        {

            if (dto == null) throw new ArgumentException("Debe enviar los datos requeridos para reservar el turno.");

            // validar que el usuario este logueado
            Usuario searchUser = await _usuarioRepository.GetUsuarioByIdAsync(dto.UsuarioId);
            if (searchUser == null ) throw new KeyNotFoundException("El usuario no existe");
            
            // validar que el turno exista en el local y servicio correspondiente
            Local local = await _localRepository.GetLocalById(dto.LocalId);
            if (local == null) throw new KeyNotFoundException("El local no existe");
         
            Servicio servicio =  await _servicioRepository.GetServiceById(dto.ServicioId);
            if (servicio == null) throw new KeyNotFoundException("El servicio no existe");
                
            // validar que existe un turno disponible en la fecha y hora solicitada
            // HorarioAtencion horario = _localRepository.GetHorarioByLocalId(dto.LocalId);
            DayOfWeek diaReserva = dto.Date.DayOfWeek;
            var horarioDia = local.HorariosAtencion.FirstOrDefault(h => h.DiaSemana == diaReserva);
            if (horarioDia == null || horarioDia.EstaCerrado) throw new InvalidOperationException("El local se encuentra cerrado el día seleccionado.");

            // Extraer el horario de atención del local para el día de la reserva
            TimeSpan horaTurno = dto.Date.TimeOfDay;
            if(horaTurno < horarioDia.HoraApertura || horaTurno > horarioDia.HoraCierre) throw new InvalidOperationException($"El local atiende de {horarioDia.HoraApertura:hh\\:mm} a {horarioDia.HoraCierre:hh\\:mm}.");

            // Verificar si ya existe un turno reservado para el mismo local, servicio y fecha
            DateTime fechaUtc = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc);
            bool turnoExistente = await _turnoRespository.AppointmentExistsAsync(dto.LocalId, fechaUtc);
            if (turnoExistente) throw new InvalidOperationException("Ya existe un turno reservado para este local en la fecha y hora seleccionadas");

            Turno turno = new Turno{
                Date = DateTime.SpecifyKind(dto.Date,DateTimeKind.Utc),
                LocalId = dto.LocalId,
                ServicioId = dto.ServicioId,
                UsuarioId = dto.UsuarioId,
                EstaPedido = dto.EstaPedido,
            };
            
            await _turnoRespository.Add(turno);

            AddTurnoResponseDto response = _mapper.Map<AddTurnoResponseDto>(turno);
            response.Message = "Turno reservado ";
         
            return response;
        }
    }
}



