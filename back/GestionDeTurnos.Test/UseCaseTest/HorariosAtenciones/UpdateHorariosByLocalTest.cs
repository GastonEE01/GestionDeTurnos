using AutoMapper;
using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Application.UseCase.Horarios;
using GestionDeTurnos.Domain.Entities;
using GestionDeTurnos.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace GestionDeTurnos.Test.UseCaseTest.HorariosAtenciones
{
    public class UpdateHorariosByLocalTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHorarioAtencionRepository> _repositorioMock;
        private readonly Mock<ILocalRepository> _repositorioMockLocal;

        private readonly UpdateHorariosByLocalUseCase _useCase;

        public UpdateHorariosByLocalTest()
        {
            _mapperMock = new Mock<IMapper>();
            _repositorioMock = new Mock<IHorarioAtencionRepository>();
            _repositorioMockLocal = new Mock<ILocalRepository>();
            _useCase = new UpdateHorariosByLocalUseCase(_repositorioMock.Object, _repositorioMockLocal.Object, _mapperMock.Object);

        }

        /*
         🟢 Camino feliz: Pasan los 7 días ordenados con horas válidas y actualiza con éxito.
         🔴 Validación de horas cruzadas: Se envía una horaCierre anterior a la horaApertura (ej. abre 18:00 y cierra 09:00) y falla la validación.
         🔴 Permisos: Un usuario intenta modificar los horarios de un local que no le pertenece.
         🔴 Local inexistente: Se intenta actualizar el horario de un localId que no existe.
        */
        [Fact]
        public async Task ActualizarLosHorarios()
        {
            // Arrange
            var localId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            // 1) lo que existe actualmente en la base de datos
            var horariosBdd = new List<HorarioAtencion>
            {
                new HorarioAtencion {
                    LocalId = localId,
                    DiaSemana = DayOfWeek.Monday,
                    HoraApertura = TimeSpan.FromHours(9),
                    HoraCierre = TimeSpan.FromHours(18),
                    EstaCerrado = false
                }
            };

            // 2) lo que envia el cliente para actualizar (REQUEST DTO)
            var requestDto = new List<HorarioAtencionRequestDto>
            {
                new HorarioAtencionRequestDto
                {
                    DiaSemana = DayOfWeek.Monday,
            HoraApertura = TimeSpan.FromHours(10), // Cambió la hora
            HoraCierre = TimeSpan.FromHours(19),
            EstaCerrado = false
                }
            };

            // 3) Lo que espera devolver la respuesta mapeada (RESPONSE DTO)
            var responseDto = new List<HorarioAtencionResponseDto> {
                new HorarioAtencionResponseDto
                {
                    LocalId = localId,
            DiaSemana = DayOfWeek.Monday,
            HoraApertura = TimeSpan.FromHours(10),
            HoraCierre = TimeSpan.FromHours(19),
            EstaCerrado = false
                }
            };

            // Configurar MOCKS
            // A) Simular que el repositorio encuentra el local y devuelve lo que hay en BD
            _repositorioMock
                .Setup(repo => repo.GetHorarioByLocalId(localId))
                .ReturnsAsync(horariosBdd);

            // B) Simular que el Mapper convierte la lista de entidades en la lista ResponseDto
            _mapperMock
                .Setup(mapper => mapper.Map<List<HorarioAtencionResponseDto>>(horariosBdd))
                .Returns(responseDto);

            // Act
            // Le pasamos el requestDto
            var result = await _useCase.UpdateHorariosByLocalID(localId, usuarioId, requestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(localId, result[0].LocalId);

            // Verificamos que se haya guardado en BD
            _repositorioMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task ValidarHorasCruzadas()
        {
            // Arrange
            var localId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            // 1) lo que existe actualmente en la base de datos
            var horariosBdd = new List<HorarioAtencion>
            {
                new HorarioAtencion {
                    LocalId = localId,
                    DiaSemana = DayOfWeek.Monday,
                    HoraApertura = TimeSpan.FromHours(7),
                    HoraCierre = TimeSpan.FromHours(19),
                    EstaCerrado = false
                }
            };

            // Enviamos una hora de cierre ANTERIOR a la apertura
            var requestDtoCruzado = new List<HorarioAtencionRequestDto>
    {
        new HorarioAtencionRequestDto
        {
            DiaSemana = DayOfWeek.Monday,
            HoraApertura = TimeSpan.FromHours(18), // 18:00
            HoraCierre = TimeSpan.FromHours(9),    // 09:00 (Inválido)
            EstaCerrado = false
        }
    };


            _repositorioMock
        .Setup(repo => repo.GetHorarioByLocalId(localId))
        .ReturnsAsync(horariosBdd);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _useCase.UpdateHorariosByLocalID(localId, usuarioId, requestDtoCruzado));

        }

        [Fact]
        public async Task UpdateHorariosByLocalID_CuandoLocalNoExiste_DebeLanzarKeyNotFoundException()
        {
            // Arrange
            var localExistenteId = Guid.NewGuid();
            var localInexistenteId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();

            var requestDto = new List<HorarioAtencionRequestDto>
    {
        new HorarioAtencionRequestDto
        {
            DiaSemana = DayOfWeek.Monday,
            HoraApertura = TimeSpan.FromHours(10),
            HoraCierre = TimeSpan.FromHours(19),
            EstaCerrado = false
        }
    };

            // Configuramos el mock SOLO para el ID que existe
            _repositorioMock
                .Setup(repo => repo.GetHorarioByLocalId(localExistenteId))
                .ReturnsAsync(new List<HorarioAtencion>());

            // Act & Assert
            // Intentar actualizar con un ID distinto debe lanzar la excepción
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _useCase.UpdateHorariosByLocalID(localInexistenteId, usuarioId, requestDto));
        }


        //          🔴 Permisos: Un usuario intenta modificar los horarios de un local que no le pertenece.
        [Fact]
        public async Task UpdateHorariosByLocalID_usuarioIntentaModificarLosHorariosDeUnLocalQueNoExiste()
        {
            // existen 2 locales y 1 horario 
            var localId = Guid.NewGuid();
            var usuarioDuenioId = Guid.NewGuid();
            var usuarioAtacanteId = Guid.NewGuid();

            var local = new Local { Id = localId, UsuarioId = usuarioDuenioId };
            var horariosBdd = new List<HorarioAtencion> { new HorarioAtencion { LocalId = localId } };

            // Mock 1: Devuelve el local con su dueño original
            _repositorioMockLocal
                .Setup(repo => repo.GetLocalById(localId))
                .ReturnsAsync(local);

            // Mock 2: Devuelve los horarios para que no explote por KeyNotFoundException
            _repositorioMock
                .Setup(repo => repo.GetHorarioByLocalId(localId))
                .ReturnsAsync(horariosBdd);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
        _useCase.UpdateHorariosByLocalID(localId, usuarioAtacanteId, new List<HorarioAtencionRequestDto>()));

        }
    }
}

/*
 * 
 * 
            var horariosExistentesBD = new List<HorarioAtencion>
    {
        new HorarioAtencion
        {
            LocalId = localId,
            DiaSemana = DayOfWeek.Monday,
            HoraApertura = TimeSpan.FromHours(9),
            HoraCierre = TimeSpan.FromHours(18),
            EstaCerrado = false
        }
    };



            var response = new List<HorarioAtencionResponseDto>
            {
                new HorarioAtencionResponseDto
                {
                   LocalId = localId,
            DiaSemana = DayOfWeek.Tuesday,
            HoraApertura = TimeSpan.FromHours(7),
            HoraCierre = TimeSpan.FromHours(17),
            EstaCerrado = false
                }
            };

            _repositorioMock
                .Setup(repo => repo.UpdateHorario(monday))
                .ReturnsAsync(monday);

            _mapperMock
            .Setup(mapper => mapper.Map<List<HorarioAtencionResponseDto>>(monday))
            .Returns(response);

            // Act 
            var result = await _useCase.UpdateHorariosByLocalID(localId,response);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(localId, result[0].LocalId);
*/