using AutoMapper;
using GestionDeTurnos.Application.DTOs.Local;
using GestionDeTurnos.Application.DTOs.Usuario;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.Mapper
{
    public class MapperUsuario : Profile
    {
        public MapperUsuario() 
        {
            CreateMap<Usuario, UserResponseDto>().ReverseMap();

        }
    }
}
