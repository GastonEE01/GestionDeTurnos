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
        private readonly TurnoRespository _turnoRespository;

        public AddTurnoUseCase(TurnoRespository turnoRespository)
        {
            _turnoRespository = turnoRespository;
        }
    

    public Turno AddTurno(TurnoRequestDto dto)
        {
            // Mapear 

            Turno turno = new Turno{
                Date = DateTime.SpecifyKind(dto.Date,DateTimeKind.Utc),
                LocalId = dto.LocalId,
                ServicioId = dto.ServicioId,
                UsuarioId = dto.UsuarioId,
            };
            _turnoRespository.addTurno(turno);
            if (turno == null)
            {
                throw new Exception("Error al pedir un turno");
            }
            return turno;
        }
    }
}
