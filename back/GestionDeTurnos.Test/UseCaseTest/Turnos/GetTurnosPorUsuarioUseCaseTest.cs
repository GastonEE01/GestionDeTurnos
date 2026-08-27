using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Application.UseCase.Turnos;
using GestionDeTurnos.Domain.Entities;
using GestionDeTurnos.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GestionDeTurnos.Test.UseCaseTest.Turnos
{
    public class GetTurnosPorUsuarioUseCaseTest
    {
        private readonly Mock<ITurnoRespository> _turnoRespository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly GetTurnosPorUsuarioUseCase _useCase;

        public GetTurnosPorUsuarioUseCaseTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _turnoRespository = new Mock<ITurnoRespository>();
            _useCase = new GetTurnosPorUsuarioUseCase(_userRepository.Object, _turnoRespository.Object);
        }

        [Fact]
        public async Task ObtenerLosTurnosDelUsuario()
        {
            // Arrange
            Guid idLocal = Guid.NewGuid();

            Usuario usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Name = "Gaston",
                Rol = "Cliente",
            };

             List<HorarioAtencion> horarios = new List<HorarioAtencion>
             {
                 new HorarioAtencion
                 {
                     Id = Guid.NewGuid(),
                     EstaCerrado = false,
                     HoraApertura = TimeSpan.FromHours(7),
                     HoraCierre = TimeSpan.FromHours(19),
                     LocalId = idLocal,
                     DiaSemana = DayOfWeek.Monday
                 },

                  new HorarioAtencion
                 {
                     Id = Guid.NewGuid(),
                     EstaCerrado = false,
                     HoraApertura = TimeSpan.FromHours(7),
                     HoraCierre = TimeSpan.FromHours(19),
                     LocalId = idLocal,
                     DiaSemana = DayOfWeek.Tuesday
                 }
             };

            List<Servicio> servicios = new List<Servicio>
            {
                new Servicio
            {
                Id = Guid.NewGuid(),
                LocalId = idLocal,
                Name = "Servicio",
                DurationInMinutes = 30,
                Price = 10000
            }
            };

        Local local = new Local
            {
                Id = idLocal,
                Name = "Local de pepe",
                UsuarioId = Guid.NewGuid(),
                HorariosAtencion = horarios,
                Servicios = servicios
        };

            List<Turno> turnos = new List<Turno> {
                new Turno
                {
                    Id = Guid.NewGuid(),
                    EstaPedido = false,
                    UsuarioId = usuario.Id,
                    LocalId = idLocal,
                    Servicio = servicios[0],
                    Date = DateTime.Now.AddHours(7.5)
                }
                };

            _turnoRespository.Setup(t => t.GetTurnosByUsuarioIdAsync(usuario.Id)).ReturnsAsync(turnos);
            _userRepository.Setup(t => t.GetUsuarioByIdAsync(usuario.Id)).ReturnsAsync(usuario);

            // Act
            var result = await _useCase.GetTurnosByUser(usuario.Id);

            // Assert
            Assert.NotNull(result);

            _turnoRespository.Verify(r => r.GetTurnosByUsuarioIdAsync(usuario.Id), Times.Once);
            _userRepository.Verify(r => r.GetUsuarioByIdAsync(usuario.Id), Times.Once);

        }

        [Fact]
        public async Task ElUsuarioNoExiste()
        {
            // Arrange
            Guid idLocal = Guid.NewGuid();

            List<HorarioAtencion> horarios = new List<HorarioAtencion>
             {
                 new HorarioAtencion
                 {
                     Id = Guid.NewGuid(),
                     EstaCerrado = false,
                     HoraApertura = TimeSpan.FromHours(7),
                     HoraCierre = TimeSpan.FromHours(19),
                     LocalId = idLocal,
                     DiaSemana = DayOfWeek.Monday
                 },

                  new HorarioAtencion
                 {
                     Id = Guid.NewGuid(),
                     EstaCerrado = false,
                     HoraApertura = TimeSpan.FromHours(7),
                     HoraCierre = TimeSpan.FromHours(19),
                     LocalId = idLocal,
                     DiaSemana = DayOfWeek.Tuesday
                 }
             };

            List<Servicio> servicios = new List<Servicio>
            {
                new Servicio
            {
                Id = Guid.NewGuid(),
                LocalId = idLocal,
                Name = "Servicio",
                DurationInMinutes = 30,
                Price = 10000
            }
            };

            Local local = new Local
            {
                Id = idLocal,
                Name = "Local de pepe",
                UsuarioId = Guid.NewGuid(),
                HorariosAtencion = horarios,
                Servicios = servicios
            };

            List<Turno> turnos = new List<Turno> {
                new Turno
                {
                    Id = Guid.NewGuid(),
                    EstaPedido = false,
                    LocalId = idLocal,
                    Servicio = servicios[0],
                    Date = DateTime.Now.AddHours(7.5)
                }
                };

            Guid idUsuarioNoExistente = Guid.NewGuid();

            // Le decimos a Moq que cuando pregunten por ese ID, devuelva NULL
            _userRepository.Setup(r => r.GetUsuarioByIdAsync(idUsuarioNoExistente)).ReturnsAsync((Usuario)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.GetTurnosByUser(idUsuarioNoExistente));
            Assert.Equal("El usuario no existe", exception.Message);

            // Verify: 
            _userRepository.Verify(r => r.GetUsuarioByIdAsync(idUsuarioNoExistente), Times.Once);
            _turnoRespository.Verify(r => r.GetTurnosByUsuarioIdAsync(idUsuarioNoExistente), Times.Never);
        }

        [Fact]
        public async Task ElUsuarioNoTieneTurnoRegistrados()
        {
            // Arrange
            Guid usuarioId = Guid.NewGuid();
            Usuario usuario = new Usuario { Id = usuarioId, Name = "Gaston" };

            _userRepository
                .Setup(r => r.GetUsuarioByIdAsync(usuarioId))
                .ReturnsAsync(usuario);

            // Simulamos que el repositorio devuelve una lista VACÍA
            _turnoRespository
                .Setup(t => t.GetTurnosByUsuarioIdAsync(usuarioId))
                .ReturnsAsync(new List<Turno>());

            // Act
            var result = await _useCase.GetTurnosByUser(usuarioId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // 🟢 Verifica que la lista tenga 0 elementos

            _userRepository.Verify(r => r.GetUsuarioByIdAsync(usuarioId), Times.Once);
            _turnoRespository.Verify(r => r.GetTurnosByUsuarioIdAsync(usuarioId), Times.Once);
        }



    }
}
