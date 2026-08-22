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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestionDeTurnos.Test.UseCaseTest.Turnos
{
    public class GetTurnosSlotUseCaseTest
    {
        private readonly Mock<IHorarioAtencionRepository> _horarioAtencionRepository;
        private readonly Mock<ILocalRepository> _localRepository;
        private readonly Mock<IServicioRepository> _servicioRepository;
        private readonly Mock<ITurnoRespository> _turnoRepository;

        private readonly GetTurnosSlotUseCase _useCase;

        public GetTurnosSlotUseCaseTest()
        {
            _localRepository = new Mock<ILocalRepository>();
            _servicioRepository = new Mock<IServicioRepository>();
            _horarioAtencionRepository = new Mock<IHorarioAtencionRepository>();
            _turnoRepository = new Mock<ITurnoRespository>();
            _useCase = new GetTurnosSlotUseCase(_horarioAtencionRepository.Object, _localRepository.Object,_servicioRepository.Object, _turnoRepository.Object);
        }

        [Fact]
        public async Task MostrarHorariosDisponibles()
        {
            // arrange
            Guid idLocal = Guid.NewGuid();
            Guid idServicio = Guid.NewGuid();
            Guid idTurno = Guid.NewGuid();
            Guid idHorario = Guid.NewGuid();
            Guid idHorario2 = Guid.NewGuid();
            Guid idUsuario = Guid.NewGuid();

            DateTime hoy = DateTime.Today;
            int diasHastaElLunes = ((int)DayOfWeek.Monday - (int)hoy.DayOfWeek + 7) % 7;
            DateTime diaTurno = hoy.AddDays(diasHastaElLunes);

            // se necesita un local que tenga serrivicios y los dias que habre 

            List<Servicio> servicios = new List<Servicio>
            {
                new Servicio{
                Id = idServicio,
                Description = "description",
                DurationInMinutes = 60,
                LocalId = idLocal,
                Price = 15000
                }
            };

            List<HorarioAtencion> horarios = new List<HorarioAtencion>
            {
                new HorarioAtencion
                {
                    Id = idHorario,
                    LocalId=idLocal,
                    DiaSemana = DayOfWeek.Monday,
                    EstaCerrado = false,
                    HoraApertura = TimeSpan.FromHours(8),
                    HoraCierre = TimeSpan.FromHours(18),
                },
                 new HorarioAtencion
                {
                    Id = idHorario2,
                    LocalId=idLocal,
                    DiaSemana = DayOfWeek.Tuesday,
                    EstaCerrado = false,
                    HoraApertura = TimeSpan.FromHours(8),
                    HoraCierre = TimeSpan.FromHours(18),
                }
            };
            
            Local local = new Local
                {
                Id = idLocal,
                Name = "Local de juan",
                Servicios = servicios,
                HorariosAtencion = horarios,          
                };

            // creamos un turno para mostrar los horarios disponibles con su usuario
            Usuario usuario = new Usuario
            {
                Id = idUsuario,
                Name = "Gaston",
                Rol = "Cliente",
            };

            List<Turno> turnos = new List<Turno>
            {
                new Turno
                {
                Id = idTurno,
                Date = diaTurno.Date.AddHours(10),
                EstaPedido = true,
                LocalId = idLocal,
                ServicioId = idServicio,
                UsuarioId = idUsuario
                }
               
            };
            
            // act 
           _localRepository.Setup(l => l.GetLocalById(idLocal)).ReturnsAsync(local);
           _servicioRepository.Setup(s => s.GetServiceById(idServicio)).ReturnsAsync(servicios[0]);
            _localRepository.Setup(h => h.GetHorarioByLocalId(idLocal)).ReturnsAsync(horarios[0]);
            _turnoRepository.Setup(t => t.GetTurnosByLocalAndFechaAsync(idLocal, It.IsAny<DateTime>())).ReturnsAsync(turnos);

            var result = await _useCase.GetTurnos(idLocal, idServicio, diaTurno);


            var horariosEsperados = new List<TimeSpan>
{
    new TimeSpan(8, 0, 0),
    new TimeSpan(9, 0, 0),
    // 10:00 NO debería estar porque está ocupado por el turno
    new TimeSpan(11, 0, 0),
    new TimeSpan(12, 0, 0),
    new TimeSpan(13, 0, 0),
    new TimeSpan(14, 0, 0),
    new TimeSpan(15, 0, 0),
    new TimeSpan(16, 0, 0),
    new TimeSpan(17, 0, 0) // El último slot posible antes del cierre de las 18:00
};

            // assert 
            // que el resultado no sea null y chequear que el local tenga un servicio con sus horarios 
            Assert.NotNull(result);
            Assert.Equal(horariosEsperados.Count, result.Count);
            Assert.Equal(horariosEsperados, result);
            Assert.DoesNotContain(new TimeSpan(10, 0, 0), result);

            // verificar si los repos se ejecutaron 
            _localRepository.Verify(r => r.GetLocalById(idLocal), Times.Once);
            _servicioRepository.Verify(r => r.GetServiceById(idServicio), Times.Once);
            _localRepository.Verify(r => r.GetHorarioByLocalId(idLocal), Times.Once);
            _turnoRepository.Verify(r => r.GetTurnosByLocalAndFechaAsync(idLocal, diaTurno), Times.Once);
        }

        [Fact]
        public async Task MostrarHorariosDisponibles_ServicioInexistente_LanzaExcepcion()
        {
            Guid idHorario = Guid.NewGuid();
            Guid idHorario2 = Guid.NewGuid();
            Guid idLocal = Guid.NewGuid();
            Guid idServicio = Guid.NewGuid();
            Guid idUsuario = Guid.NewGuid();
            Guid idTurno = Guid.NewGuid();

            DateTime hoy = DateTime.Today;
            int diasHastaElLunes = ((int)DayOfWeek.Monday - (int)hoy.DayOfWeek + 7) % 7;
            DateTime diaTurno = hoy.AddDays(diasHastaElLunes);

            List<HorarioAtencion> horarios = new List<HorarioAtencion>
            {
                new HorarioAtencion
                {
                    Id = idHorario,
                    LocalId=idLocal,
                    DiaSemana = DayOfWeek.Monday,
                    EstaCerrado = false,
                    HoraApertura = TimeSpan.FromHours(8),
                    HoraCierre = TimeSpan.FromHours(18),
                },
                 new HorarioAtencion
                {
                    Id = idHorario2,
                    LocalId=idLocal,
                    DiaSemana = DayOfWeek.Tuesday,
                    EstaCerrado = false,
                    HoraApertura = TimeSpan.FromHours(8),
                    HoraCierre = TimeSpan.FromHours(18),
                }
            };

            Local local = new Local
            {
                Id = idLocal,
                Name = "Local de juan",
                Servicios = null,
                HorariosAtencion = horarios,
            };

            Usuario usuario = new Usuario
            {
                Id = idUsuario,
                Name = "Gaston",
                Rol = "Cliente",
            };

            List<Turno> turnos = new List<Turno>
            {
                new Turno
                {
                Id = idTurno,
                Date = diaTurno.Date.AddHours(10),
                EstaPedido = true,
                LocalId = idLocal,
                ServicioId = idServicio,
                UsuarioId = idUsuario
                }

            };

            _localRepository.Setup(l => l.GetLocalById(idLocal)).ReturnsAsync(local);
            _servicioRepository.Setup(s => s.GetServiceById(idServicio)).ReturnsAsync((Servicio)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.GetTurnos(idLocal, idServicio, diaTurno));
            Assert.Equal("Este local no tiene servicios", exception.Message);

            // Assert
            _localRepository.Verify(r => r.GetLocalById(idLocal), Times.Once);
            _servicioRepository.Verify(r => r.GetServiceById(idServicio), Times.Once);

        }

        [Fact]
        public async Task MostrarHorariosDisponibles_LocalCerrado_LanzaExcepcion()
        {
            Guid idHorario = Guid.NewGuid();
            Guid idHorario2 = Guid.NewGuid();
            Guid idLocal = Guid.NewGuid();
            Guid idServicio = Guid.NewGuid();
            Guid idUsuario = Guid.NewGuid();
            Guid idTurno = Guid.NewGuid();

            DateTime hoy = DateTime.Today;
            int diasHastaElLunes = ((int)DayOfWeek.Monday - (int)hoy.DayOfWeek + 7) % 7;
            DateTime diaTurno = hoy.AddDays(diasHastaElLunes);

            List<HorarioAtencion> horarios = new List<HorarioAtencion>
            {
                new HorarioAtencion
                {
                    Id = idHorario,
                    LocalId=idLocal,
                    DiaSemana = DayOfWeek.Monday,
                    EstaCerrado = true,
                    HoraApertura = TimeSpan.FromHours(8),
                    HoraCierre = TimeSpan.FromHours(18),
                },
                 new HorarioAtencion
                {
                    Id = idHorario2,
                    LocalId=idLocal,
                    DiaSemana = DayOfWeek.Tuesday,
                    EstaCerrado = false,
                    HoraApertura = TimeSpan.FromHours(8),
                    HoraCierre = TimeSpan.FromHours(18),
                }
            };

            List<Servicio> servicios = new List<Servicio>
            {
                new Servicio{
                Id = idServicio,
                Description = "description",
                DurationInMinutes = 60,
                LocalId = idLocal,
                Price = 15000
                }
            };

            Local local = new Local
            {
                Id = idLocal,
                Name = "Local de juan",
                Servicios = servicios,
                HorariosAtencion = horarios,    
            };

            Usuario usuario = new Usuario
            {
                Id = idUsuario,
                Name = "Gaston",
                Rol = "Cliente",
            };

            List<Turno> turnos = new List<Turno>
            {
                new Turno
                {
                Id = idTurno,
                Date = diaTurno.Date.AddHours(10),
                EstaPedido = true,
                LocalId = idLocal,
                ServicioId = idServicio,
                UsuarioId = idUsuario
                }

            };

            _localRepository.Setup(l => l.GetLocalById(idLocal)).ReturnsAsync(local);
            _servicioRepository.Setup(s => s.GetServiceById(idServicio)).ReturnsAsync(servicios[0]);
            _localRepository.Setup(l => l.GetHorarioByLocalId(idLocal)).ReturnsAsync(horarios[0]);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.GetTurnos(idLocal, idServicio, diaTurno));
            Assert.Equal("El local esta cerrado", exception.Message);

            // Assert
            _localRepository.Verify(r => r.GetLocalById(idLocal), Times.Once);
            _servicioRepository.Verify(r => r.GetServiceById(idServicio), Times.Once);
            _localRepository.Verify(r => r.GetHorarioByLocalId(idLocal), Times.Once);

        }
    }
}
