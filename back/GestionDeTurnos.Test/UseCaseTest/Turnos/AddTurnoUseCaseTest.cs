using GestionDeTurnos.Application.DTOs.Turno;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Application.UseCase.Turnos;
using GestionDeTurnos.Domain.Entities;
using GestionDeTurnos.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace GestionDeTurnos.Test.UseCaseTest.Turnos
{
    public class AddTurnoUseCaseTest
    {
        private readonly Mock<ITurnoRespository> _turnoRespositoryMock;
        private readonly Mock<IUserRepository> _usuarioRepositoryMock;
        private readonly Mock<ILocalRepository> _localRepositoryMock;
        private readonly AddTurnoUseCase _useCase;

        public AddTurnoUseCaseTest()
        {
            _turnoRespositoryMock = new Mock<ITurnoRespository>();
            _usuarioRepositoryMock = new Mock<IUserRepository>();
            _localRepositoryMock =  new Mock<ILocalRepository>();
         //   _useCase = new AddTurnoUseCase(_turnoRespositoryMock.Object, _usuarioRepositoryMock.Object, _localRepositoryMock.Object);
        }

    /*    [Fact]
        public async Task UsuarioClientePideTurno()
        {
            // preparar
            // Necesito un TurnoRequestDto
            var UsuarioId = Guid.NewGuid();
            var ServicioId = Guid.NewGuid();
            var LocalId = Guid.NewGuid();


            var fechaReservaValida = new DateTime(2026, 8, 24, 10, 0, 0);

            AddTurnoRequestDto request = new AddTurnoRequestDto
                {
                Date = fechaReservaValida,
                EstaPedido = false,
                LocalId = LocalId,
                ServicioId = ServicioId,
                UsuarioId = UsuarioId,
                };

           
            Usuario usuario = new Usuario
            {
                Id = UsuarioId,
                Name = "Gaston",
                Email = "gaston@gmail.com",
                Rol = "Cliente"
            };

            var horarios = new List<HorarioAtencion>
            {
                 new HorarioAtencion
                 {
                     Id = Guid.NewGuid(),
                     EstaCerrado = false,
                     LocalId = LocalId,
                     DiaSemana = DayOfWeek.Monday,
                     HoraApertura = TimeSpan.FromHours(7),
                     HoraCierre = TimeSpan.FromHours(17),
                 }
                };
           

            // Asociamos un horario al local para saber si esta abierto 
            Local local = new Local
            {
                Id = LocalId,
                Name = "local nombre",
                Category = "Juguetes",
                HorariosAtencion = horarios

            };

            Servicio servicio = new Servicio
            {
                Id = ServicioId,
                Name = "servio nombre",
                Description = "masaje",
                DurationInMinutes = 40,
                LocalId = LocalId,
                Price = 100.00M,
            };

            Turno turno = new Turno
            {
                Id = Guid.NewGuid(),
                EstaPedido = false,
                LocalId = LocalId,
                ServicioId = ServicioId,
                UsuarioId = UsuarioId,
            };

            // metodos que uso GetUsuarioByIdAsync,GetLocalById y AppointmentExistsAsync 

            // Configurar respuesta
            _usuarioRepositoryMock.Setup(r => r.GetUsuarioByIdAsync(UsuarioId)).ReturnsAsync(usuario);
            _localRepositoryMock.Setup(s => s.GetLocalById(LocalId)).ReturnsAsync(local);
            _localRepositoryMock.Setup(s => s.GetServicioById(ServicioId)).Returns(servicio);
            _turnoRespositoryMock.Setup(t => t.AppointmentExistsAsync(LocalId, It.IsAny<DateTime>())).ReturnsAsync(false);
           
            // Ejecutar 
            var result = await _useCase.AddTurno(request); // linea 92

            // Verificar
            Assert.NotNull(result);
            _turnoRespositoryMock.Verify(r => r.Add(It.IsAny<Turno>()),Times.Once);


        }

        //   1. Turno Ocupado (Validación de disponibilidad)
        //   Objetivo: Verificar que si la fecha / hora solicitada ya fue reservada por otra persona, el Use Case rechace la solicitud.
        [Fact]
        public async Task LanzarExcepcionDeTurnoOcupado()
        {
            var UsuarioId = Guid.NewGuid();
            var ServicioId = Guid.NewGuid();
            var LocalId = Guid.NewGuid();


            var fechaReservaValida = new DateTime(2026, 8, 24, 10, 0, 0);

            AddTurnoRequestDto request = new AddTurnoRequestDto
            {
                Date = fechaReservaValida,
                EstaPedido = true,
                LocalId = LocalId,
                ServicioId = ServicioId,
                UsuarioId = UsuarioId,
            };

            Usuario usuario = new Usuario
            {
                Id = UsuarioId,
                Name = "Gaston",
                Email = "gaston@gmail.com",
                Rol = "Cliente"
            };

            var horarios = new List<HorarioAtencion>
            {
                 new HorarioAtencion
                 {
                     Id = Guid.NewGuid(),
                     EstaCerrado = false,
                     LocalId = LocalId,
                     DiaSemana = DayOfWeek.Monday,
                     HoraApertura = TimeSpan.FromHours(7),
                     HoraCierre = TimeSpan.FromHours(17),
                 }
            };

            // Asociamos un horario al local para saber si esta abierto 
            Local local = new Local
            {
                Id = LocalId,
                Name = "local nombre",
                Category = "Juguetes",
                HorariosAtencion = horarios
            };

            Servicio servicio = new Servicio
            {
                Id = ServicioId,
                Name = "servio nombre",
                Description = "masaje",
                DurationInMinutes = 40,
                LocalId = LocalId,
                Price = 100.00M,
            };

            Turno turno = new Turno
            {
                Id = Guid.NewGuid(),
                EstaPedido = true,
                LocalId = LocalId,
                ServicioId = ServicioId,
                UsuarioId = UsuarioId,
            };

            // metodos que uso GetUsuarioByIdAsync,GetLocalById y AppointmentExistsAsync 

            // Configurar respuesta
            _usuarioRepositoryMock.Setup(r => r.GetUsuarioByIdAsync(UsuarioId)).ReturnsAsync(usuario);
            _localRepositoryMock.Setup(s => s.GetLocalById(LocalId)).ReturnsAsync(local);
            _localRepositoryMock.Setup(s => s.GetServicioById(ServicioId)).Returns(servicio);
            _turnoRespositoryMock.Setup(t => t.AppointmentExistsAsync(LocalId, It.IsAny<DateTime>())).ReturnsAsync(true);

            // Ejecutar 
           // var result = await _useCase.AddTurno(request); // linea 92

            // Verificar
          //  Assert.NotNull(result);
            await Assert.ThrowsAsync<ArgumentException>(() =>  _useCase.AddTurno(request));
            _turnoRespositoryMock.Verify(r => r.Add(It.IsAny<Turno>()), Times.Never);

        }

        // 2. Local Cerrado (Validación de días de atención)
        // Objetivo: Verificar que si el cliente pide un turno para un día que el local no abre(ej.solicita un domingo y el local solo abre de lunes a viernes), el Use Case tire error.

        [Fact]
        public async Task LocalCerrado()
        {
            // preparar
            // Necesito un TurnoRequestDto
            var UsuarioId = Guid.NewGuid();
            var ServicioId = Guid.NewGuid();
            var LocalId = Guid.NewGuid();


            var fechaReservaValida = new DateTime(2026, 8, 24, 10, 0, 0);

            AddTurnoRequestDto request = new AddTurnoRequestDto
            {
                Date = fechaReservaValida,
                EstaPedido = false,
                LocalId = LocalId,
                ServicioId = ServicioId,
                UsuarioId = UsuarioId,
            };


            Usuario usuario = new Usuario
            {
                Id = UsuarioId,
                Name = "Gaston",
                Email = "gaston@gmail.com",
                Rol = "Cliente"
            };

            var horarios = new List<HorarioAtencion>
            {
                 new HorarioAtencion
                 {
                     Id = Guid.NewGuid(),
                     EstaCerrado = true,
                     LocalId = LocalId,
                     DiaSemana = DayOfWeek.Monday,
                     HoraApertura = TimeSpan.FromHours(7),
                     HoraCierre = TimeSpan.FromHours(17),
                 }
                };


            // Asociamos un horario al local para saber si esta abierto 
            Local local = new Local
            {
                Id = LocalId,
                Name = "local nombre",
                Category = "Juguetes",
                HorariosAtencion = horarios

            };

            Servicio servicio = new Servicio
            {
                Id = ServicioId,
                Name = "servio nombre",
                Description = "masaje",
                DurationInMinutes = 40,
                LocalId = LocalId,
                Price = 100.00M,
            };

            Turno turno = new Turno
            {
                Id = Guid.NewGuid(),
                EstaPedido = false,
                LocalId = LocalId,
                ServicioId = ServicioId,
                UsuarioId = UsuarioId,
            };

            // metodos que uso GetUsuarioByIdAsync,GetLocalById y AppointmentExistsAsync 

            // Configurar respuesta
            _usuarioRepositoryMock.Setup(r => r.GetUsuarioByIdAsync(UsuarioId)).ReturnsAsync(usuario);
            _localRepositoryMock.Setup(s => s.GetLocalById(LocalId)).ReturnsAsync(local);
            _localRepositoryMock.Setup(s => s.GetServicioById(ServicioId)).Returns(servicio);
            _turnoRespositoryMock.Setup(t => t.AppointmentExistsAsync(LocalId, It.IsAny<DateTime>())).ReturnsAsync(false);

           await Assert.ThrowsAsync<ArgumentException>(() => _useCase.AddTurno(request));
           _turnoRespositoryMock.Verify(r => r.Add(It.IsAny<Turno>()), Times.Never);

        }

        // 3. Horario Fuera de Rango (Validación de horario de apertura/cierre)
        // Objetivo: Verificar que si piden un turno un lunes a las 05:00 AM o a las 23:00 PM(fuera del rango de 07:00 a 17:00), el sistema lo rechace.

        [Fact]
        public async Task UsuarioPideTurnoFueraDeRango()
        {
            // preparar
            // Necesito un TurnoRequestDto
            var UsuarioId = Guid.NewGuid();
            var ServicioId = Guid.NewGuid();
            var LocalId = Guid.NewGuid();


            var fechaReservaValida = new DateTime(2026, 8, 24, 5, 0, 0);

            AddTurnoRequestDto request = new AddTurnoRequestDto
            {
                Date = fechaReservaValida,
                EstaPedido = false,
                LocalId = LocalId,
                ServicioId = ServicioId,
                UsuarioId = UsuarioId,
            };


            Usuario usuario = new Usuario
            {
                Id = UsuarioId,
                Name = "Gaston",
                Email = "gaston@gmail.com",
                Rol = "Cliente"
            };

            var horarios = new List<HorarioAtencion>
            {
                 new HorarioAtencion
                 {
                     Id = Guid.NewGuid(),
                     EstaCerrado = false,
                     LocalId = LocalId,
                     DiaSemana = DayOfWeek.Monday,
                     HoraApertura = TimeSpan.FromHours(7),
                     HoraCierre = TimeSpan.FromHours(17),
                 }
                };


            // Asociamos un horario al local para saber si esta abierto 
            Local local = new Local
            {
                Id = LocalId,
                Name = "local nombre",
                Category = "Juguetes",
                HorariosAtencion = horarios

            };

            Servicio servicio = new Servicio
            {
                Id = ServicioId,
                Name = "servio nombre",
                Description = "masaje",
                DurationInMinutes = 40,
                LocalId = LocalId,
                Price = 100.00M,
            };

            Turno turno = new Turno
            {
                Id = Guid.NewGuid(),
                EstaPedido = false,
                LocalId = LocalId,
                ServicioId = ServicioId,
                UsuarioId = UsuarioId,
            };

            // metodos que uso GetUsuarioByIdAsync,GetLocalById y AppointmentExistsAsync 

            // Configurar respuesta
            _usuarioRepositoryMock.Setup(r => r.GetUsuarioByIdAsync(UsuarioId)).ReturnsAsync(usuario);
            _localRepositoryMock.Setup(s => s.GetLocalById(LocalId)).ReturnsAsync(local);
            _localRepositoryMock.Setup(s => s.GetServicioById(ServicioId)).Returns(servicio);
            _turnoRespositoryMock.Setup(t => t.AppointmentExistsAsync(LocalId, It.IsAny<DateTime>())).ReturnsAsync(false);

            await Assert.ThrowsAsync<ArgumentException>(() => _useCase.AddTurno(request));
            _turnoRespositoryMock.Verify(r => r.Add(It.IsAny<Turno>()), Times.Never);
        }

        // 4. Horario Fuera de Rango (Validación de horario de apertura/cierre)
        // Objetivo: Verificar que si piden un turno un lunes a las 05:00 AM o a las 23:00 PM(fuera del rango de 07:00 a 17:00), el sistema lo rechace.

        

        // 5. Obtener/Calcular Horarios Disponibles (Slots)
        // Objetivo: Probar el método que arma la lista de bloques de horarios(ej. 10:00, 10:40, 11:20) filtrando los que ya fueron reservados en la base de datos.

     
        */
    }
}
