using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Application.UseCase.Horarios;
using GestionDeTurnos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeTurnos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioAtencionController : ControllerBase
    {
        private readonly GetHorariosByLocal _getHorariosByLocal;
        private readonly UpdateHorariosByLocal _updateHorariosByLocal;

        public HorarioAtencionController(GetHorariosByLocal getHorariosByLocal, UpdateHorariosByLocal updateHorariosByLocal)
        {
            _getHorariosByLocal = getHorariosByLocal;
            _updateHorariosByLocal = updateHorariosByLocal;
        }

        [HttpGet("{localId}")]
        public async Task<IActionResult> GetHorariosByLocal(Guid localId)
        {
            try
            {
                var horarios = await _getHorariosByLocal.GetHorariosAtencionByLocal(localId);
                return Ok(horarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{localId}")]
        public async Task<IActionResult> UpdateHorariosByLocal(Guid localId, [FromBody] List<HorarioAtencionRequestDto> horarios)
        {
            try
            {
                List<HorarioAtencionResponseDto> response = await _updateHorariosByLocal.UpdateHorariosByLocalID(localId, horarios);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // 404 NOT FOUND            }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message }); // 400 BAD REQUEST
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // 409 CONFLICT
            }
        }

    }
}
