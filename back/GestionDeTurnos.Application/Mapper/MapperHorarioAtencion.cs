using AutoMapper;
using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.Mapper
{
    public class MapperHorarioAtencion : Profile
    {
        public MapperHorarioAtencion()
        {
            CreateMap<HorarioAtencion, HorarioAtencionResponseDto>().ReverseMap();
        }
    }
}
