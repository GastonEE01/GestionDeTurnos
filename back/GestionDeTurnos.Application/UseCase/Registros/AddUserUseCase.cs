using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Registros
{
    public class AddUserUseCase 
    {
        private readonly IUserReposirtory _userReposirtory;

        public AddUserUseCase(IUserReposirtory userRepository)
        {
            _userReposirtory = userRepository;
        }
        public Usuario addUser(UserRequestDto dto)
        {
            // validar datos

            // validar  el gmail
            if (!dto.Email.Contains("@") || !dto.Email.EndsWith("gmail.com"))
                throw new ValidationException("El email debe ser una direccion valida de Gmail(debe contener '@' y terminar en gmail.com.");

            // validar  la contraseña 
            bool containMayus = dto.Password.Any(Char.IsUpper);
            bool containMinus = dto.Password.Any(Char.IsLower);
            bool containNumber = dto.Password.Any(Char.IsDigit);

            if (dto.Password.Length <= 5 || !containMayus || !containMinus || !containNumber)
                throw new ValidationException("La contraseña debe tener 6 o mas caracteres y contener una letra mayuscula,una minuscula y un numero");

            if (!dto.Password.Equals(dto.ConfirmPassword))
                throw new ValidationException("La contraseña no coincide con la contraseña confirmada");

            var passworHasher = new PasswordHasher<Usuario>();
            var user = new Usuario
            {
                Name = dto.Name,
                Email = dto.Email,
                Rol = dto.Rol
            };

            // Hashear la contraseña 
            string passworHasherConfirm = passworHasher.HashPassword(user, dto.Password);

            user.PasswordHash = passworHasherConfirm;
            _userReposirtory.Add(user);


            return user;
           

        }

    }
}
