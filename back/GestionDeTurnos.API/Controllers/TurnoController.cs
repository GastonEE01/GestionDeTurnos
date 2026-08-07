using GestionDeTurnos.Application.DTOs;
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
        public TurnoController(AddTurnoUseCase addTurnoUseCase) {
            _addTurnoUseCase = addTurnoUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> AddTurno(TurnoRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Error al pedir turno");
            }

            try
            {
                Turno turno = await _addTurnoUseCase.AddTurno(dto);
                TurnoResponseDto response = new TurnoResponseDto
                {
                    Id = turno.Id,
                    Date = turno.Date,
                    Message = "Turno creado con exito"
                };
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                // Muestra errores de validación de negocio (400 Bad Request)
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // 🚨 409 CONFLICT cuando el turno ya existe
                return Conflict(new { message = ex.Message });
            }

        }
    }
}
