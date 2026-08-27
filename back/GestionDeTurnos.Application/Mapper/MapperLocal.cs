using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GestionDeTurnos.Application.DTOs.Local;
using GestionDeTurnos.Application.DTOs.Servicio;
using GestionDeTurnos.Application.DTOs.HorarioAtencion;


namespace GestionDeTurnos.Application.Mapper
{
    public class MapperLocal : Profile
    {
        public MapperLocal()
        {
            CreateMap<Local, AddLocalResponseDto>().ReverseMap();
            CreateMap<LocalRequestDto, Local>().ReverseMap();

            CreateMap<HorarioAtencion, HorarioAtencionResponseDto>().ReverseMap(); CreateMap<Servicio, GetServicioByLocalResponseDto>();
            CreateMap<HorarioAtencion, HorarioAtencionRequestDto>().ReverseMap();

        }

    }
}
