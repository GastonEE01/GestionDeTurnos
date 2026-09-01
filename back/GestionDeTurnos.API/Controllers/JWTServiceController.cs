using GestionDeTurnos.Application.DTOs.Usuario;
using GestionDeTurnos.Application.UseCase.Usuarios;
using GestionDeTurnos.Domain.Entities;
using GestionDeTurnos.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeTurnos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JWTServiceController : ControllerBase
    {
        private readonly UserLoginUseCase _userLoginUseCase;
        public JWTServiceController(UserLoginUseCase userLoginUseCase)
        {
            _userLoginUseCase = userLoginUseCase;
        }

       
        [HttpPost("Login")]
        public async Task<IActionResult> LoginPrueba([FromBody] LoginRequestDto dto)
        {
            var response = await _userLoginUseCase.Login(dto);
            return Ok(response);         
        }
    }
}
