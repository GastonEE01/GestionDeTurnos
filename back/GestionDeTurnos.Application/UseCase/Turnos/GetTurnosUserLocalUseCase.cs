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
        private readonly IUserRepository _userRepository;         
        private readonly IServicioRepository _servicioRepository; 

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

            var turnos = await _turnoRespository.GetTurnosByLocalIdAsync(localId);

            if (turnos == null) return new List<TurnoDto>();

            var turnosDto = new List<TurnoDto>();

            foreach (var t in turnos)
            {
                var cliente = await _userRepository.GetUsuarioByIdAsync(t.UsuarioId); 
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
        Console.WriteLine($"ERROR EN GetTurnosByLocal: {ex.Message} --- StackTrace: {ex.StackTrace}");
        throw;
    }
}


    }
}