using GestionDeTurnos.Application.DTOs.Servicio;
using GestionDeTurnos.Application.UseCase.Servicios;
using GestionDeTurnos.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeTurnos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly AddServiceUseCase _crearServicio;
        private readonly GetServiceByLocal _getServiceByLocal;
        private readonly DeleteServiceUseCase _deleteService;
        private readonly UpdateServiceUseCase _updateService;

        public ServicioController(AddServiceUseCase crearServicioUseCase, GetServiceByLocal getServiceByLocal, DeleteServiceUseCase deleteService, UpdateServiceUseCase updateService   )
        {
            _crearServicio = crearServicioUseCase;
            _getServiceByLocal = getServiceByLocal;
            _deleteService = deleteService;
            _updateService = updateService;
        }

        [HttpPost("{localId}/servicios")]
        public async Task<IActionResult> AddService(Guid localId, AddServicioRequestDto dto)
        {    
              var response = await _crearServicio.AddService(dto,localId);
              return Ok(response);
        }

        [HttpGet("{localId}/servicios")]
        public async Task<IActionResult> GetServiciosByLocal(Guid localId)
        {
                var servicios = await _getServiceByLocal.GetServiciosByLocal(localId);
                return Ok(servicios);    
        }

        [HttpDelete("{localId}/servicios/{servicioId}")]
        public async Task<IActionResult> DeleteService(Guid localId, Guid servicioId)
        {
            await _deleteService.DeleteService(servicioId);
            return Ok(new { message = "Servicio eliminado exitosamente" });                 
        }

        [HttpPut("{localId}/servicios/{servicioId}")]
        public async Task<IActionResult> UpdateService(Guid localId, Guid servicioId, UpdateServicioRequestDto dto)
        { 
                var response = await _updateService.UpdateService(localId ,servicioId, dto);  
                return Ok(response);  
        }
    }
}
