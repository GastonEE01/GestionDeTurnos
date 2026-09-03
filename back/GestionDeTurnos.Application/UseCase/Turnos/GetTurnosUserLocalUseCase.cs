using GestionDeTurnos.Application.DTOs.TurnoDTO;
using GestionDeTurnos.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Turnos
{
    public class GetTurnosUserLocalUseCase
    {
        private readonly ILocalRepository _localRepository;
        private readonly ITurnoRespository _turnoRespository;
        private readonly IUserRepository _userRepository;         // Para saber el nombre del cliente que sacó el turno
        private readonly IServicioRepository _servicioRepository; // Para saber el nombre del servicio

        public GetTurnosUserLocalUseCase(
            ILocalRepository localRepository,
            ITurnoRespository turnoRespository,
            IUserRepository userRepository,
            IServicioRepository servicioRepository)
        {
            _localRepository = localRepository;
            _turnoRespository = turnoRespository;
            _userRepository = userRepository;
            _servicioRepository = servicioRepository;
        }

        public async Task<List<TurnoDto>> GetTurnosByLocal(Guid localId)
        {
            try { 
            var local = await _localRepository.GetLocalById(localId);
            if (local == null) throw new Exception("El local no existe");

            // Traemos todos los turnos que pertenecen a este local
            var turnos = await _turnoRespository.GetTurnosByLocalIdAsync(localId);

            if (turnos == null) return new List<TurnoDto>();

            var turnosDto = new List<TurnoDto>();

            foreach (var t in turnos)
            {
                // Buscamos quién es el cliente que sacó el turno para mostrar su nombre
                var cliente = await _userRepository.GetUsuarioByIdAsync(t.UsuarioId); // (Asegúrate de que tu entidad Turno tenga el UsuarioId)
                var servicio = await _servicioRepository.GetServiceById(t.ServicioId);

                turnosDto.Add(new TurnoDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    LocalId = t.LocalId,
                    ServicioId = t.ServicioId,
                    ServicioName = servicio?.Name ?? "Servicio desconocido",
                    LocalName = cliente?.Name ?? "Cliente desconocido",
                    EstaPedido = t.EstaPedido
                });
            }

            return turnosDto;
        }catch (Exception ex)
    {
        // 🛑 ESTO TE IMPRIMIRÁ EL ERROR EXACTO EN TU TERMINAL DE C#
        Console.WriteLine($"ERROR EN GetTurnosByLocal: {ex.Message} --- StackTrace: {ex.StackTrace}");
        throw;
    }
}


    }
}