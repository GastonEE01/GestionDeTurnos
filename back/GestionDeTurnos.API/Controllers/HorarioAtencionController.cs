using GestionDeTurnos.Application.DTOs.HorarioAtencion;
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
            var horarios = await _getHorariosByLocal.GetHorariosAtencionByLocal(localId);
            return Ok(horarios);
            
            
        }

        [HttpPut("{localId}")]
        [Authorize]
        public async Task<IActionResult> UpdateHorariosByLocal(Guid localId, [FromBody] List<HorarioAtencionRequestDto> horarios)
        {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                List<HorarioAtencionResponseDto> response = await _updateHorariosByLocal.UpdateHorariosByLocalID(localId,userId, horarios);
                return Ok(response);

        }

    }
}
