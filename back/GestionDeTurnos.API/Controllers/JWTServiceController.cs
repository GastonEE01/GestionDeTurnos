using GestionDeTurnos.Application.DTOs;
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
        private readonly JWTService _service;
        private readonly GetUserUseCase _getUser;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public JWTServiceController(JWTService service,GetUserUseCase getUser)
        {
            _service = service;
            _getUser = getUser;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

       
        [HttpPost("Login")]
        public async Task<IActionResult> LoginPrueba([FromBody] LoginRequestDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                {
                    return BadRequest("El email y la contraseña son obligatorios.");
                }

                Usuario searchUser = await _getUser.GetUser(dto.Email);

                if (searchUser == null)
                {
                    return BadRequest("Credenciales incorrectas.");
                }

                var validationPassword = _passwordHasher.VerifyHashedPassword(searchUser, searchUser.PasswordHash, dto.Password);

                if (validationPassword == PasswordVerificationResult.Failed)
                {
                    return BadRequest("Credenciales incorrectas.");
                }

                string token = _service.GenerateToken(searchUser.Id.ToString(), searchUser.Email, searchUser.Rol);
                LoginResponseDto response = new LoginResponseDto
                {
                    Id = searchUser.Id,
                    Email = searchUser.Email,
                    Name = searchUser.Name,
                    Rol = searchUser.Rol,
                };
                return Ok(new { token = token, response });
            }
            catch (Exception ex)
            {
                return BadRequest("Usuario no encontrado");
            }


          
        }
    }
}
