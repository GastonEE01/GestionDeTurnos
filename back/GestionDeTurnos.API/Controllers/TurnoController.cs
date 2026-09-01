using GestionDeTurnos.Application.DTOs.Turno;
using GestionDeTurnos.Application.DTOs.TurnoDTO;
using GestionDeTurnos.Application.UseCase.Turnos;
using GestionDeTurnos.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeTurnos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnoController : ControllerBase
    {
        private readonly  AddTurnoUseCase _addTurnoUseCase;
        private readonly CancelTurnoUseCase _cancelTurnoUseCase;
        private readonly GetTurnosPorUsuarioUseCase _getTurnosPorUsuarioUseCase;
        private readonly GetTurnosSlotUseCase _getTurnosSlotUseCase;

        public TurnoController(AddTurnoUseCase addTurnoUseCase,CancelTurnoUseCase cancelTurnoUseCase,GetTurnosPorUsuarioUseCase getTurnosPorUsuarioUseCase,GetTurnosSlotUseCase getTurnosSlotUseCase) {
            _addTurnoUseCase = addTurnoUseCase;
            _cancelTurnoUseCase= cancelTurnoUseCase;
            _getTurnosPorUsuarioUseCase = getTurnosPorUsuarioUseCase;
            _getTurnosSlotUseCase = getTurnosSlotUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> AddTurno([FromBody] AddTurnoRequestDto dto)
        {
            dto.Date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc);
            var response = await _addTurnoUseCase.AddTurno(dto);
                return Ok(response);
        }

        [HttpDelete("{turnoId}/cancelar")]
        public async Task<IActionResult> CancelTurno(Guid turnoId, Guid usuarioId) 
        {                
            await _cancelTurnoUseCase.CancelTurno(turnoId, usuarioId);
            return Ok(new { message = "Turno cancelado" });    
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetTurnosUser(Guid usuarioId)
        {
            GetTurnosUsuarioResponse turnos = await _getTurnosPorUsuarioUseCase.GetTurnosByUser(usuarioId);
            return Ok(turnos);
        }

        [HttpGet("Disponibles")]
        public async Task<IActionResult> GetTurnosSlot([FromQuery] Guid localId, [FromQuery] Guid servicioId, [FromQuery] DateTime fecha)
        {
            List<TimeSpan> turnosSlot = await _getTurnosSlotUseCase.GetTurnos(localId,servicioId,fecha);
            return Ok(turnosSlot);  
        }
    }
}
