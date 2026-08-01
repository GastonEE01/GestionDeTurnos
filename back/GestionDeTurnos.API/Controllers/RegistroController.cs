using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Application.UseCase.Registros;
using GestionDeTurnos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeTurnos.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RegistroController : ControllerBase
    {

        private readonly AddUserUseCase _addUsers;

        public RegistroController(AddUserUseCase addUsers)
        {
            _addUsers = addUsers;
        }

        [HttpPost]
        public IActionResult AddUser(UserRequestDto dto)
        {
            if (dto == null)
            {
               return BadRequest("Error al pedir turno");
            }
            try
            {
                Usuario newUser = _addUsers.addUser(dto);
                UserResponseDto response = new UserResponseDto
                {
                    Id = newUser.Id,
                    Name = newUser.Name,
                    Email = newUser.Email,
                    Rol = newUser.Rol
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            }

    }
}
