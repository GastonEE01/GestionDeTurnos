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
        private readonly GetLocalUseCase _getLocales;
        private readonly AddLocalUseCase _addLocal;
        private readonly IMapper _mapper;
        public LocalController(GetLocalUseCase getLocalUseCase, AddLocalUseCase addLocalUseCase, IMapper mapper)
        {
            _getLocales = getLocalUseCase;
            _addLocal = addLocalUseCase;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetLocales()
        {
            try
            {
                var locales = _getLocales.GetLocal();
                return Ok(locales);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost]
        public ActionResult AddLocal(CrearLocalRequest dto)
        {
            if (dto.Local == null || dto.Horarios == null)
            {
                return BadRequest("No se proporcionaron objetos válidos.");
            }

            try
            {
                var local = _addLocal.AddLocal(dto.Local, dto.Horarios);
                // Como 'LocalResponseDto' tiene listas de DTOs y NO tiene propiedades de navegación 
                // hacia atrás, el ciclo infinito desaparece por completo.
                var localResponse = _mapper.Map<LocalResponseDto>(local);
                return Ok(localResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

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
