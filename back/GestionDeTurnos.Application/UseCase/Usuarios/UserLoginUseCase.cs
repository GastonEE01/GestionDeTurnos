using GestionDeTurnos.Application.DTOs.Usuario;
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
    public class UserLoginUseCase
    {
        public readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _service;

        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UserLoginUseCase(IUserRepository userRepository,IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _service = jwtTokenGenerator;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            
           if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password)) throw new ArgumentException("El email y la contraseña son obligatorios.");
            
            Usuario searchUser = await _userRepository.GetUserAsync(dto.Email);

            if (searchUser == null) throw new KeyNotFoundException("No se encontro al usuario.");
             
            var validationPassword = _passwordHasher.VerifyHashedPassword(searchUser, searchUser.PasswordHash, dto.Password);

                if (validationPassword == PasswordVerificationResult.Failed) throw new KeyNotFoundException("Contraseña incorrecta.");


                string token = _service.GenerateToken(searchUser.Id.ToString(), searchUser.Email, searchUser.Rol);
                LoginResponseDto response = new LoginResponseDto
                {
                    Id = searchUser.Id,
                    Email = searchUser.Email,
                    Name = searchUser.Name,
                    Rol = searchUser.Rol,
                    Token = token,
                };

            return response;
        }
    }
}
