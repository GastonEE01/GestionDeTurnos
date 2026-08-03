using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Usuarios
{
    public class GetUserUseCase
    {
        private readonly IUserRepository _userReposirtory;

        public GetUserUseCase(IUserRepository userReposirtory)
        {
            _userReposirtory = userReposirtory;
        }

        public Usuario GetUser(string email)
        {
            // Buscar al usuario 
            Usuario user = _userReposirtory.GetUser(email);

            if (user == null)
            {
                throw new Exception("No se encontro al usuario"); 
            }

            return user;

        }

       

    }
}
