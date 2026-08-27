using GestionDeTurnos.Application.DTOs.Turno;
using GestionDeTurnos.Application.DTOs.TurnoDTO;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Turnos
{
    public class GetTurnosPorUsuarioUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITurnoRespository _turnoRespository;

        public GetTurnosPorUsuarioUseCase(IUserRepository userRepository, ITurnoRespository turnoRespository)
        {
            _userRepository = userRepository;
            _turnoRespository = turnoRespository;
        }

        public async Task<GetTurnosUsuarioResponse> GetTurnosByUser(Guid usuarioId)
        {
            var user = await _userRepository.GetUsuarioByIdAsync(usuarioId);
            if (user == null) throw new Exception("El usuario no existe");

            var turnos = await _turnoRespository.GetTurnosByUsuarioIdAsync(usuarioId);

            return new GetTurnosUsuarioResponse
            {
                UsuarioId = user.Id,
                NameUser = user.Name, // si tu entidad Usuario tiene esta propiedad
                Turnos = turnos.Select(t => new TurnoDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    LocalId = t.LocalId,
                    ServicioId = t.ServicioId,
                    EstaPedido = t.EstaPedido
                }).ToList()
            };
        }
    }
}