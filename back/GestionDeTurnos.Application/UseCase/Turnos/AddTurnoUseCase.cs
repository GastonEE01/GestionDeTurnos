using GestionDeTurnos.Application.DTOs;
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
        public AddTurnoUseCase(ITurnoRespository turnoRespository, IUserRepository usuarioRepository, ILocalRepository localRepository)
        {
            _turnoRespository = turnoRespository;
            _usuarioRepository = usuarioRepository;
            _localRepository = localRepository;
        }
    

    public async Task<Turno> AddTurno(TurnoRequestDto dto)
        {
            // Mapear 
            // validar que el usuario este logueado
            Usuario searchUser = await _usuarioRepository.GetUsuarioByIdAsync(dto.UsuarioId);
            if (searchUser == null )
            {
                throw new ArgumentException("El usuario no existe", nameof(dto.UsuarioId));
            }
            
            // validar que el turno exista en el local y servicio correspondiente
            Local local = await _localRepository.GetLocalById(dto.LocalId);
            if (local == null)
            {
                throw new ArgumentException("El local no existe", nameof(dto.LocalId));
            }
          
            Servicio servicio =  _localRepository.GetServicioById(dto.ServicioId);
                if (servicio == null)
                {
                    throw new ArgumentException("El servicio no existe", nameof(dto.ServicioId));
                }


            // validar que existe un turno disponible en la fecha y hora solicitada
            // HorarioAtencion horario = _localRepository.GetHorarioByLocalId(dto.LocalId);
            DayOfWeek diaReserva = dto.Date.DayOfWeek;
            var horarioDia = local.HorariosAtencion.FirstOrDefault(h => h.DiaSemana == diaReserva);
            if (horarioDia == null || horarioDia.EstaCerrado)
            {
                throw new ArgumentException("El local se encuentra cerrado el día seleccionado.", nameof(dto.Date));

            }
            // Extraer el horario de atención del local para el día de la reserva
            TimeSpan horaTurno = dto.Date.TimeOfDay;
            if(horaTurno < horarioDia.HoraApertura || horaTurno > horarioDia.HoraCierre)
            {
                throw new ArgumentException($"El local atiende de {horarioDia.HoraApertura} a {horarioDia.HoraCierre}.", nameof(dto.Date));
            }

            // Verificar si ya existe un turno reservado para el mismo local, servicio y fecha
            DateTime fechaUtc = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc);
            bool turnoExistente = await _turnoRespository.AppointmentExistsAsync(dto.LocalId, fechaUtc);
            if (turnoExistente)
            {
                throw new ArgumentException("Ya existe un turno reservado para este local en la fecha y hora seleccionadas.", nameof(dto.Date));
            }


            Turno turno = new Turno{
                Date = DateTime.SpecifyKind(dto.Date,DateTimeKind.Utc),
                LocalId = dto.LocalId,
                ServicioId = dto.ServicioId,
                UsuarioId = dto.UsuarioId,
                EstaPedido = dto.EstaPedido,
            };
            await _turnoRespository.addTurnoAsync(turno);
            if (turno == null)
            {
                throw new Exception("Error al pedir un turno");
            }
            return turno;
        }
    }
}



/*public class TurnoRespositoryEf : ITurnoRespository
{
    private readonly YourDbContext _context;

    public TurnoRespositoryEf(YourDbContext context)
    {
        _context = context;
    }

    public Turno addTurno(Turno dto)
    {
        _context.Turnos.Add(dto);
        _context.SaveChanges();
        return dto;
    }

    public bool ExisteTurno(Guid localId, Guid servicioId, DateTime date)
    {
        // comparar según la lógica de tu modelo (fecha exacta u rango)
        return _context.Turnos.Any(t => t.LocalId == localId
                                     && t.ServicioId == servicioId
                                     && t.Date == date);
    }*/


