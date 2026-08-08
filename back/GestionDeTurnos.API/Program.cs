using AutoMapper;
using Azure.Identity;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Application.UseCase.Horarios;
using GestionDeTurnos.Application.UseCase.Locales;
using GestionDeTurnos.Application.UseCase.Registros;
using GestionDeTurnos.Application.UseCase.Servicios;
using GestionDeTurnos.Application.UseCase.Turnos;
using GestionDeTurnos.Application.UseCase.Usuarios;
using GestionDeTurnos.Infrastructure.Data;
using GestionDeTurnos.Infrastructure.Repositories;
using GestionDeTurnos.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Repositorios
builder.Services.AddScoped<ILocalRepository, LocalRepository>();
builder.Services.AddScoped<ITurnoRespository, TurnoRepository>();
builder.Services.AddScoped<IUserRepository, UsuarioRepository>();
builder.Services.AddScoped<IHorarioAtencionRepository, HorarioAtencionRepository>();
builder.Services.AddScoped<IServicioRepository, ServicioRepository>();

// Servicios
builder.Services.AddScoped<JWTService>();

// Casos de uso
builder.Services.AddScoped<AddLocalUseCase>();
builder.Services.AddScoped<GetLocalUseCase>();
builder.Services.AddScoped<GetLocalByIdLocalUseCase>();
builder.Services.AddScoped<GetLocalesByUsuarioIdUseCase>();
builder.Services.AddScoped<DeleteLocalByIdUseCase>();
builder.Services.AddScoped<UpdateLocalUseCase>();

builder.Services.AddScoped<AddTurnoUseCase>();
builder.Services.AddScoped<AddUserUseCase>();
builder.Services.AddScoped<GetUserUseCase>();

builder.Services.AddScoped<AddServiceUseCase>();
builder.Services.AddScoped<UpdateServiceUseCase>();
builder.Services.AddScoped<DeleteServiceUseCase>();
builder.Services.AddScoped<GetServiceByLocal>();

builder.Services.AddScoped<GetHorariosByLocal>();
builder.Services.AddScoped<UpdateHorariosByLocal>();



// // 🔌 Le enseñamos a .NET cómo construir el IMapper usando tu clase de mapeo
builder.Services.AddAutoMapper(cfg => { }, typeof(GestionDeTurnos.Application.Mapper.MapperLocal));

// Configuracion de la Base de Datos (PostgreSQL con Neon)
var connectionString = Environment.GetEnvironmentVariable("NeonTech__connectionString");

if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetSection("NeonTech:connectionString").Value;
}
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("No se encontró la cadena de conexión 'NeonTech' en ningún entorno.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(connectionString));

// Configuracion de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresá: Bearer {tu token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configuracion del JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "TuEmisorGenérico",      // Después lo pasamos al appsettings.json
            ValidAudience = "TuAudienciaGenérica",  // Después lo pasamos al appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("UnaClaveSuperSecretaYMuyLargaDeMasDe32Caracteres!"))
        };
    });

// Telemetría de Application Insights
builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

// Configuracion de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",                  // Tu React en tu PC
                "https://app-peliculas-three.vercel.app"   // Tu React publicado en Vercel
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// CONSTRUCCIÓN DE LA APLICACIÓN Y MIDDLEWARES
var app = builder.Build();

// Habilitar CORS como primer paso en el pipeline HTTP
app.UseCors("AllowFrontend");

// Configuración del entorno de Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = string.Empty;
    });

app.UseHttpsRedirection();

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

// MIGRACIONES AUTOMÁTICAS(Para Neon en la nube
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.Run();