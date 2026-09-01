using GestionDeTurnos.Application.DTOs.Local;
using GestionDeTurnos.Application.DTOs.Usuario;
using GestionDeTurnos.Application.UseCase.Locales;
using GestionDeTurnos.Application.UseCase.Registros;
using GestionDeTurnos.Domain.Entities;
using GestionDeTurnos.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDeTurnos.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RegistroController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AddUserUseCase _addUsers;
        private readonly AddLocalUseCase _addLocals;


        public RegistroController(AppDbContext context, AddUserUseCase addUsers, AddLocalUseCase addLocals)
        {
            _context = context;
            _addUsers = addUsers;
            _addLocals = addLocals;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserRequestDto dto)
        {
            var response = await _addUsers.AddUser(dto);
            return Ok(response);
        }

        [HttpPost("Registro-Local")]
        public async Task<IActionResult> AddLocal([FromBody] RegisterLocalRequest dto)
        {
            // Iniciamos una transacción atómica
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var userResponse = await _addUsers.AddUser(dto.User);

                dto.Local.UsuarioId = userResponse.Id;

                var localResponse = await _addLocals.AddLocal(dto.Local, dto.Horarios);

                // Si ambos pasaron sin excepciones, confirmamos los cambios
                await transaction.CommitAsync();

                return Ok(localResponse);
            }
            catch (Exception ex)
            {
                // Si algo falla en CUALQUIERA de los dos pasos, se revierte todo
                await transaction.RollbackAsync();

                return BadRequest(new { Message = ex.Message });
            }

        }
    }
}

