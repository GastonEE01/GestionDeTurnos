using AutoMapper;
using GestionDeTurnos.Application.DTOs;
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

        private readonly IMapper _mapper;
        public LocalController(GetLocalUseCase getLocalUseCase, GetLocalesByUsuarioIdUseCase getLocalesByUsuarioId ,GetLocalByIdLocalUseCase getLocalByIdLocal, DeleteLocalByIdUseCase deleteLocalById, AddLocalUseCase addLocalUseCase, UpdateLocalUseCase updateLocalUseCase, IMapper mapper)
        {
            _getLocales = getLocalUseCase;
            _getLocalesByUsuarioId = getLocalesByUsuarioId;
            _getLocalById = getLocalByIdLocal;
            _deleteLocalById = deleteLocalById;
            _addLocal = addLocalUseCase;
            _updateLocal = updateLocalUseCase;
            //_mapper = mapper;     
        }

        [HttpGet]
        public async Task<ActionResult> GetLocales()
        {
            try
            {
                var locales = await _getLocales.GetLocal();
                return Ok(locales);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        // para usar en el perfil del local, para mostrar los datos del local y sus horarios
        [HttpGet("{idLocal}")]
        public async Task<IActionResult> GetLocalById(Guid idLocal)
        {
            try
            {
                var local = await _getLocalById.GetLocalById(idLocal);
                if (local == null)
                {
                    return NotFound($"No se encontró un local con el ID {idLocal}.");
                }
                return Ok(local);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetLocalesByUsuario(Guid usuarioId)
        {
            try
            {
                var locales = await _getLocalesByUsuarioId.GetLocalesByUsuario(usuarioId);
                return Ok(locales);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{idLocal}")]
        public async Task<ActionResult> DeleteLocal(Guid idLocal)
        {
            try
            {
               await _deleteLocalById.DeleteLocal(idLocal);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult> AddLocal(CrearLocalRequest dto)
        {
            if (dto.Local == null || dto.Horarios == null)
            {
                return BadRequest("No se proporcionaron objetos válidos.");
            }

            try
            {
                var local = await _addLocal.AddLocal (dto.Local, dto.Horarios);
                // Como 'LocalResponseDto' tiene listas de DTOs y NO tiene propiedades de navegación 
                // hacia atrás, el ciclo infinito desaparece por completo.
                // aca
                return Ok(local);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            }
            catch (InvalidOperationException ex)
            {
                // 🚨 409 CONFLICT cuando el turno ya existe
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{idLocal}")]
        public async Task<ActionResult> modifyLocal([FromRoute] Guid idLocal,[FromBody] UpdateLocalRequestDto dto)
        {
            try
            {
                var response = await _updateLocal.ExecuteAsync(idLocal, dto);
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



        // GET: LocalController/Details/5
        /*  public ActionResult Details(int id)
          {
              return View();
          }

          // GET: LocalController/Create
          public ActionResult Create()
          {
              return View();
          }

          // POST: LocalController/Create
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Create(IFormCollection collection)
          {
              try
              {
                  return RedirectToAction(nameof(Index));
              }
              catch
              {
                  return View();
              }
          }

          // GET: LocalController/Edit/5
          public ActionResult Edit(int id)
          {
              return View();
          }

          // POST: LocalController/Edit/5
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Edit(int id, IFormCollection collection)
          {
              try
              {
                  return RedirectToAction(nameof(Index));
              }
              catch
              {
                  return View();
              }
          }

          // GET: LocalController/Delete/5
          public ActionResult Delete(int id)
          {
              return View();
          }

          // POST: LocalController/Delete/5
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Delete(int id, IFormCollection collection)
          {
              try
              {
                  return RedirectToAction(nameof(Index));
              }
              catch
              {
                  return View();
              }
          }*/
    }
}
