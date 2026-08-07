using GestionDeTurnos.Application.DTOs;
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
        public async Task<IActionResult> AddService(Guid localId, AddServiceRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Error al crear servicio");
            }
            try
            {
                Servicio servicio = await _crearServicio.AddService(dto, localId);
                AddServiceResponseDto response = new AddServiceResponseDto
                {
                    UsuarioId = servicio.Id,
                    Name = servicio.Name,
                    Description = servicio.Description,
                    DurationInMinutes = servicio.DurationInMinutes,
                    Price = servicio.Price,
                    Message = "Servicio creado exitosamente"
                };

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                // Muestra errores de validación de negocio (400 Bad Request)
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // 🚨 409 CONFLICT cuando el servicio ya existe
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("{localId}/servicios")]
        public async Task<IActionResult> GetServiciosByLocal(Guid localId)
        {
            try
            {
                var servicios = await _getServiceByLocal.GetServiciosByLocal(localId);
                return Ok(servicios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{localId}/servicios/{servicioId}")]
        public async Task<IActionResult> DeleteService(Guid localId, Guid servicioId)
        {
            try
            {
                await _deleteService.DeleteService(servicioId);
                return Ok(new { message = "Servicio eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{localId}/servicios/{servicioId}")]
        public async Task<IActionResult> UpdateService(Guid localId, Guid servicioId, UpdateServiceRequestDto dto)
        {
            try
            {
                var response = await _updateService.UpdateService(servicioId, dto);  
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
    }
}
