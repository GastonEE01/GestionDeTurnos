using GestionDeTurnos.Application.DTOs.Usuario;
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
        public async Task<IActionResult> AddUser(UserRequestDto dto)
        {
           var response = await _addUsers.AddUser(dto);
           return Ok(response); 
        }
    }
}
