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
    public class CancelTurnoUseCase
    {
        private readonly ITurnoRespository _turnoRepository;

        public CancelTurnoUseCase(ITurnoRespository turnoRespository)
        {
            _turnoRepository = turnoRespository;
        }

        public async Task CancelTurno(Guid turnoId,Guid usuarioId)
        {
            var searchTurno= await _turnoRepository.GetByTurnoIdAsync(turnoId);

            if (searchTurno == null) throw new Exception("El turno no existe");

            if (searchTurno.UsuarioId != usuarioId) throw new UnauthorizedAccessException("No tienes permiso para cancelar este turno");
              
             DateTime fechaTurno = searchTurno.Date;
                DateTime fechaActual = DateTime.Now;
                TimeSpan diferent = fechaTurno - fechaActual;

            if (diferent.TotalHours < 24) throw new InvalidOperationException("No podés cancelar un turno con menos de 24 horas de anticipación.");

            searchTurno.EstaPedido = false;

            await _turnoRepository.Delete(searchTurno);
                }

        }
    }

