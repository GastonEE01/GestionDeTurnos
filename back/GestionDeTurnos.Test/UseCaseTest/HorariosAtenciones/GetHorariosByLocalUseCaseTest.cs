using AutoMapper;
using GestionDeTurnos.Application.DTOs;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Application.UseCase.Horarios;
using GestionDeTurnos.Application.UseCase.Locales;
using GestionDeTurnos.Domain.Entities;
using GestionDeTurnos.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace GestionDeTurnos.Test;

public class GetHorariosByLocalUseCaseTest
{
    /*
      🟢 Camino feliz: Se pasa un localId válido que existe y devuelve la lista de horarios correcta.
      🔴 Local inexistente: Se pasa un localId que no existe y devuelve un error (o lanza una excepción NotFoundException).
      🟡 Sin horarios configurados: El local existe pero nunca cargó horarios (debería devolver una lista vacía o valores por defecto).
    */
    private readonly Mock<IHorarioAtencionRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetHorariosByLocalUseCase _useCase;

    public GetHorariosByLocalUseCaseTest()
    {
        // 1. Instanciamos los Mocks de las dos dependencias
        _repositoryMock = new Mock<IHorarioAtencionRepository>();
        _mapperMock = new Mock<IMapper>();

        // 2. Inyectamos AMBOS mocks (.Object) al instanciar el Use Case
        _useCase = new GetHorariosByLocalUseCase(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetHorariosByLocalsTest()
    {
        // Arrange
        var localId = Guid.NewGuid();

        // Crear las entidades de dominio
        var horarios = new List<HorarioAtencion>
        {
        new HorarioAtencion
        {
            LocalId = localId,
            DiaSemana = DayOfWeek.Monday,
            HoraApertura = TimeSpan.FromHours(9),
            HoraCierre = TimeSpan.FromHours(18),
            EstaCerrado = false
        },
        new HorarioAtencion
        {
            LocalId = localId,
            DiaSemana = DayOfWeek.Tuesday,
            HoraApertura = TimeSpan.FromHours(9),
            HoraCierre = TimeSpan.FromHours(18),
            EstaCerrado = false
        }
    };

        var dtosEsperados = new List<HorarioAtencionResponseDto>
        {
            new HorarioAtencionResponseDto
            {
                Id = horarios[0].Id,
                LocalId = localId,
                DiaSemana = DayOfWeek.Monday,
                HoraApertura = TimeSpan.FromHours(9),
                HoraCierre = TimeSpan.FromHours(18),
                EstaCerrado = false
            }
        };

        _repositoryMock
             .Setup(repo => repo.GetHorarioByLocalId(localId))
             .ReturnsAsync(horarios);

        // Simular que AutoMapper convierte esas entidades en DTOs
        _mapperMock
            .Setup(mapper => mapper.Map<List<HorarioAtencionResponseDto>>(horarios))
            .Returns(dtosEsperados);


        // Instancear el caso de uso pasandole el mock(.Object)

        // Act
        var result = await _useCase.GetHorariosAtencionByLocal(localId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(localId, result[0].LocalId);

        // Verificamos ejecuciones
        _repositoryMock.Verify(repo => repo.GetHorarioByLocalId(localId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<List<HorarioAtencionResponseDto>>(horarios), Times.Once);
    }

    [Fact]
    public async Task GetHorariosAtencionByLocal_CuandoNoExistenHorarios_DebeLanzarKeyNotFoundException()
    {
        // Arrange
        var localId = Guid.NewGuid();

        // El repositorio devuelve una lista vacía
        _repositoryMock
            .Setup(repo => repo.GetHorarioByLocalId(localId))
            .ReturnsAsync(new List<HorarioAtencion>());

        // Act & Assert
        // Verificamos que al ejecutar el Use Case lance la excepción KeyNotFoundException
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _useCase.GetHorariosAtencionByLocal(localId));
    }

    [Fact]
    public async Task GetHorariosAtencionByLocal_CuandoLocalNoExiste()
    {
        // Arrange
        var localIdInexistente = Guid.NewGuid();

        // El repositorio devuelve un local que no existe
        _repositoryMock
            .Setup(repo => repo.GetHorarioByLocalId(localIdInexistente))
            .ReturnsAsync((List<HorarioAtencion>)null!);

        // Act & Assert
        // Verificamos que al ejecutar el Use Case lance la excepción KeyNotFoundException
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _useCase.GetHorariosAtencionByLocal(localIdInexistente));
    }
}

