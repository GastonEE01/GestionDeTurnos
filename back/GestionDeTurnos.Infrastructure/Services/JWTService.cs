using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Infrastructure.Services
{
    public class JWTService
    {

       public string GenerateToken(string usuarioId,string email,string rol)
        {
            // Informacion publica que viaja
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,usuarioId),
                new Claim(ClaimTypes.Email,email),
                new Claim(ClaimTypes.Role,rol),
            };

            // La misma clave secreta que pusiste en el programs para confirmar el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("UnaClaveSuperSecretaYMuyLargaDeMasDe32Caracteres!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Crear el objeto del token con sus propiedades
            var token = new JwtSecurityToken(
            issuer: "TuEmisorGenérico",
            audience: "TuAudienciaGenérica",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2), // Cuánto dura el login
            signingCredentials: creds
        );

            // Transformamos el objet en el string largo que react va a guardar
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
