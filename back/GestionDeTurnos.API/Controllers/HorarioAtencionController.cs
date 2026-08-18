using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Application.UseCase.Horarios;
using GestionDeTurnos.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestionDeTurnos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioAtencionController : ControllerBase
    {
        private readonly GetHorariosByLocalUseCase _getHorariosByLocal;
        private readonly UpdateHorariosByLocalUseCase _updateHorariosByLocal;

        public HorarioAtencionController(GetHorariosByLocalUseCase getHorariosByLocal, UpdateHorariosByLocalUseCase updateHorariosByLocal)
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
        [Authorize]
        public async Task<IActionResult> UpdateHorariosByLocal(Guid localId, [FromBody] List<HorarioAtencionRequestDto> horarios)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(string.IsNullOrEmpty(userIdClaim) )
                    return Unauthorized(new { message = "Usuario no autenticado." });

                var userId = Guid.Parse(userIdClaim);

                List<HorarioAtencionResponseDto> response = await _updateHorariosByLocal.UpdateHorariosByLocalID(localId,userId, horarios);
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
