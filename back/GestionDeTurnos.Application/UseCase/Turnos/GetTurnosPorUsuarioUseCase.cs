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
        private readonly ILocalRepository _localRepository;         // <-- Inyecta el repositorio de locales
        private readonly IServicioRepository _servicioRepository;

        public GetTurnosPorUsuarioUseCase(IUserRepository userRepository, ITurnoRespository turnoRespository, ILocalRepository localRepository, IServicioRepository servicioRepository)
        {
            _userRepository = userRepository;
            _turnoRespository = turnoRespository;
            _localRepository = localRepository;
            _servicioRepository = servicioRepository;
        }

        public async Task<GetTurnosUsuarioResponse> GetTurnosByUser(Guid usuarioId)
        {
            var user = await _userRepository.GetUsuarioByIdAsync(usuarioId);
            if (user == null) throw new Exception("El usuario no existe");

            var turnos = await _turnoRespository.GetTurnosByUsuarioIdAsync(usuarioId);
            var turnosDto = new List<TurnoDto>();

            foreach (var t in turnos)
            {
                // Buscamos el nombre del local y del servicio correspondientes
                var local = await _localRepository.GetLocalById(t.LocalId);
                var servicio = await _servicioRepository.GetServiceById(t.ServicioId);

                turnosDto.Add(new TurnoDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    LocalId = t.LocalId,
                    LocalName = local?.Name ?? "Local desconocido",       // <-- Asignamos nombre
                    ServicioId = t.ServicioId,
                    ServicioName = servicio?.Name ?? "Servicio desconocido", // <-- Asignamos nombre
                    EstaPedido = t.EstaPedido
                });
            }
            return new GetTurnosUsuarioResponse
            {
                UsuarioId = user.Id,
                NameUser = user.Name,
                Turnos = turnosDto
            };
            /* return new GetTurnosUsuarioResponse
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
                 }).ToList() };*/
        
        }
    }
}