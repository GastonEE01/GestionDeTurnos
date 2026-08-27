using AutoMapper;
using GestionDeTurnos.Application.DTOs.Turno;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.Mapper
{
    public class MapperTurno : Profile
    {
        public MapperTurno() 
        {
            CreateMap<Turno, AddTurnoResponseDto>().ReverseMap();
        }
    }
}
