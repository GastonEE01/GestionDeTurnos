using AutoMapper;
using GestionDeTurnos.Application.DTOs.Local;
using GestionDeTurnos.Application.UseCase.Locales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeTurnos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalController : ControllerBase
    {
        private readonly GetLocalesByUsuarioIdUseCase _getLocalesByUsuarioId;
        private readonly GetLocalUseCase _getLocales;
        private readonly GetLocalByIdLocalUseCase _getLocalById;
        private readonly DeleteLocalByIdUseCase _deleteLocalById;
        private readonly AddLocalUseCase _addLocal;
        private readonly UpdateLocalUseCase _updateLocal;

        public LocalController(GetLocalUseCase getLocalUseCase, GetLocalesByUsuarioIdUseCase getLocalesByUsuarioId ,GetLocalByIdLocalUseCase getLocalByIdLocal, DeleteLocalByIdUseCase deleteLocalById, AddLocalUseCase addLocalUseCase, UpdateLocalUseCase updateLocalUseCase)
        {
            _getLocales = getLocalUseCase;
            _getLocalesByUsuarioId = getLocalesByUsuarioId;
            _getLocalById = getLocalByIdLocal;
            _deleteLocalById = deleteLocalById;
            _addLocal = addLocalUseCase;
            _updateLocal = updateLocalUseCase;
        }

        [HttpGet]
        public async Task<ActionResult> GetLocales()
        {
            var locales = await _getLocales.GetLocal();
            return Ok(locales);   
        }

        // para usar en el perfil del local, para mostrar los datos del local y sus horarios
        [HttpGet("{idLocal}")]
        public async Task<IActionResult> GetLocalById(Guid idLocal)
        {
            var local = await _getLocalById.GetLocalById(idLocal);
            return Ok(local);    
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetLocalesByUsuario(Guid usuarioId)
        {
                var locales = await _getLocalesByUsuarioId.GetLocalesByUsuario(usuarioId);
                return Ok(locales);
        }

        [HttpDelete("{idLocal}")]
        public async Task<ActionResult> DeleteLocal(Guid idLocal)
        {
         await _deleteLocalById.DeleteLocal(idLocal);
         return Ok(new { message = "Local eliminado" });
        }

        [HttpPost("asociar-locales")]
        public async Task<IActionResult> AsociarLocalForUser([FromBody] AsociarLocalForUserRequestDto dto)
        {
                dto.Local.UsuarioId = dto.UsuarioId;
                var localResponse = await _addLocal.AddLocal(dto.Local, dto.Horarios);

                return Ok(localResponse);
            
           
        }


        [HttpPut("{idLocal}")]
        public async Task<ActionResult> modifyLocal([FromRoute] Guid idLocal,[FromBody] UpdateLocalRequestDto dto)
        {
                var response = await _updateLocal.ExecuteAsync(idLocal, dto);
                return Ok(response); 
        }
    }
}
