using AutoMapper;
using GestionDeTurnos.Application.DTOs.Servicio;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.Mapper
{
    public class MapperServicio : Profile
    {
        public MapperServicio()
        {
            CreateMap<Servicio, AddServicioResponseDto>().ReverseMap();
            CreateMap<Servicio, UpdateServicioResponseDto>().ReverseMap();

        }
    }
}
