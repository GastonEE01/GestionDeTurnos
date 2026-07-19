using GestionDeTurnos.Application.UseCase.Locales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeTurnos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalController : ControllerBase
    {
        private readonly GetLocalUseCase _getLocales;

        public LocalController(GetLocalUseCase getLocalUseCase)
        {
            _getLocales = getLocalUseCase;
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
