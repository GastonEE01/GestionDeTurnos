using GestionDeTurnos.Application.DTOs.HorarioAtencion;
using GestionDeTurnos.Application.DTOs.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.DTOs.Local
{
    public class AddLocalResponseDto
    {
          public Guid Id { get; set; }
          public string Name { get; set; } = string.Empty;
          public string Description { get; set; } = string.Empty;
          public string Category { get; set; } = string.Empty;
          public string ImageURL { get; set; } = string.Empty;
          public string Direction { get; set; } = string.Empty;
          public string Phone { get; set; } = string.Empty;
          public List<GetServicioByLocalResponseDto> Servicios { get; set; } = new();
          public List<HorarioAtencionRequestDto> HorariosAtencion { get; set; } = new();
          public string Message {  get; set; } = string.Empty;
    }
}
