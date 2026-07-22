using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;


namespace GestionDeTurnos.Application.Mapper
{
    public class MapperLocal : Profile
    {
        public MapperLocal()
        {
            CreateMap<Local, LocalDto>().ReverseMap();
            CreateMap<Servicio, ServicioDto>().ReverseMap();
        }
       
    }
}
