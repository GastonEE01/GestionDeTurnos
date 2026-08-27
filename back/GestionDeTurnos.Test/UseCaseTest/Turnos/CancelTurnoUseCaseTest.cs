using GestionDeTurnos.Application.DTOs.Turno;
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
    public class CancelTurnoUseCaseTest
    {
        private readonly Mock<ITurnoRespository> _turnoRepositoryMock;
        private readonly CancelTurnoUseCase _useCase;


        public CancelTurnoUseCaseTest()
        {
            _turnoRepositoryMock = new Mock<ITurnoRespository>();
            _useCase = new CancelTurnoUseCase(_turnoRepositoryMock.Object);
        }

        [Fact]
        public async Task UsuarioCancelaTurno()
        {
            // preparar
            // Necesito un TurnoRequestDto
            var UsuarioId = Guid.NewGuid();
            var ServicioId = Guid.NewGuid();
            var LocalId = Guid.NewGuid();
            var TurnoId = Guid.NewGuid();

            var fechaReservaValida = DateTime.Now.AddDays(3);

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
                Id = TurnoId,
                EstaPedido = true,
                LocalId = LocalId,
                ServicioId = ServicioId,
                UsuarioId = UsuarioId,
                Date = fechaReservaValida
            };

            // metodos que uso GetUsuarioByIdAsync,GetLocalById y AppointmentExistsAsync 

            // Configurar respuesta
            //_usuarioRepositoryMock.Setup(r => r.GetUsuarioByIdAsync(UsuarioId)).ReturnsAsync(usuario);
            // _localRepositoryMock.Setup(s => s.GetLocalById(LocalId)).ReturnsAsync(local);
            // _localRepositoryMock.Setup(s => s.GetServicioById(ServicioId)).Returns(servicio);
            _turnoRepositoryMock.Setup(t => t.AppointmentExistsAsync(LocalId, It.IsAny<DateTime>())).ReturnsAsync(false);
            _turnoRepositoryMock.Setup(t => t.GetByTurnoIdAsync(TurnoId)).ReturnsAsync(turno);


            // Ejecutar 
            await _useCase.CancelTurno(TurnoId, UsuarioId);
            // Verificar
            // Assert.NotNull(result);
            //_turnoRespositoryMock.Verify(r => r.addTurnoAsync(It.IsAny<Turno>()), Times.Once);

            // Ejecutar 
            // Usuario cancela turno
            //var result2 =  _useCase2.CancelTurno(TurnoId, UsuarioId);
            _turnoRepositoryMock.Verify(r => r.DeleteTurno(It.IsAny<Turno>()), Times.Once);


        }

    }
}
