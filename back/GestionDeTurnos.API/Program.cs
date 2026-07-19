using Azure.Identity;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Application.UseCase.Locales;
using GestionDeTurnos.Infrastructure.Data;
using GestionDeTurnos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Repositorios
builder.Services.AddScoped<ILocalRepository, LocalRepository>();

// Casos de uso
builder.Services.AddScoped<GetLocalUseCase>();

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

app.UseAuthorization();

app.MapControllers();

// MIGRACIONES AUTOMÁTICAS(Para Neon en la nube
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.Run();