using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Turnos
{
    public class GetTurnosSlotUseCase
    {
        private readonly ILocalRepository _localRepository;
        private readonly IServicioRepository _servicioRepository;
        private readonly IHorarioAtencionRepository _horarioAtencionRepository;
        private readonly ITurnoRespository _turnoRepository;

        public GetTurnosSlotUseCase(IHorarioAtencionRepository horarioAtencionRepository,ILocalRepository localRepository, IServicioRepository servicioRepository,ITurnoRespository turnoRepository)
        {
            _localRepository = localRepository;
            _servicioRepository = servicioRepository;
            _horarioAtencionRepository = horarioAtencionRepository;
            _turnoRepository = turnoRepository;
        }

        public async Task<List<TimeSpan>> GetTurnos(Guid localId,Guid servicioId,DateTime fecha)
        {
            // validar que existe el local,servicio y la fecha
            var searchLocal = await _localRepository.GetLocalById(localId);
            if (searchLocal == null) throw new Exception("No se encontro el local");

            var searchService = await _servicioRepository.GetServiceById(servicioId);
            if (searchService == null) throw new Exception("Este local no tiene servicios");

            HorarioAtencion horarioLocal = await _localRepository.GetHorarioByLocalId(localId);
            if (horarioLocal.EstaCerrado) throw new Exception("El local esta cerrado"); 

            // Obtener la duracion de servicio
            TimeSpan duracionServicio = TimeSpan.FromMinutes(searchService.DurationInMinutes);
            TimeSpan slotActual = horarioLocal.HoraApertura;

            // Obtener una lista de turnos para guardar los turnos disponibles 
            List<Turno> turnos = await _turnoRepository.GetTurnosByLocalAndFechaAsync(localId, fecha);
            List<TimeSpan> horariosDisponibles = new List<TimeSpan>();

            while(slotActual + duracionServicio <= horarioLocal.HoraCierre)
            {
                // A. Construir la fecha/hora completa del slot (Fecha + TimeSpan)
                DateTime fechaHoraSlot = fecha.Date.Add(slotActual);
                // evaluar si la fechaHoraSlot esta ocupado
                bool estaOcupado = false;
                foreach(var turno in turnos)
                {
                    if(turno.Date.TimeOfDay == slotActual && turno.EstaPedido)
                    {
                        estaOcupado = true;
                        break;
                    }

                }
                if(!estaOcupado)
                {
                    horariosDisponibles.Add(slotActual);      
                }
                slotActual = slotActual.Add(duracionServicio);
                
            }

            return horariosDisponibles;
        }
    }
}
