using AutoMapper;
using GestionDeTurnos.Application.DTOs.HorarioAtencion;
using GestionDeTurnos.Application.DTOs.Local;
using GestionDeTurnos.Application.DTOs.Servicio;
using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Locales
{
    public class AddLocalUseCase
    {
        private readonly ILocalRepository _localRepository;
        public readonly IHorarioAtencionRepository _horarioAtencionRepository;
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;

        public AddLocalUseCase(ILocalRepository localRepository,IHorarioAtencionRepository horarioAtencionRepository, IUserRepository userRepository,IMapper mapper)
        {
            _localRepository = localRepository;
            _horarioAtencionRepository = horarioAtencionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<AddLocalResponseDto> AddLocal(LocalRequestDto localDto, List<HorarioAtencionRequestDto> horariosDto)
        {
            if (localDto == null || horariosDto == null)
                throw new ArgumentException("No se pudo crear el local");

            if (string.IsNullOrEmpty(localDto.Name))
                throw new ArgumentException("El nombre del local es obligatorio.");

            if (string.IsNullOrEmpty(localDto.Description)) throw new ArgumentException("La descripcion del local es obligatorio.");

            if (string.IsNullOrEmpty(localDto.Category)) throw new ArgumentException("La categoria del local es obligatorio.");

            if (string.IsNullOrEmpty(localDto.Direction)) throw new ArgumentException("La direccion del local es obligatorio.");

            if (string.IsNullOrEmpty(localDto.Phone)) throw new ArgumentException("El telefono del local es obligatorio.");

            var locales = await _localRepository.GetAll();
            if (locales.Any(l => l.Name.Equals(localDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Ya existe un local con el mismo nombre.");
            }

            if (localDto.UsuarioId == Guid.Empty)
            {
                throw new InvalidOperationException("El local debe estar asociado a un usuario válido.");
            }

            var local = new Local
            {
                Id = Guid.NewGuid(),
                Name = localDto.Name,
                Description = localDto.Description,
                Category = localDto.Category,
                ImageURL = localDto.ImageURL,
                Direction = localDto.Direction,
                Phone = localDto.Phone,
                Servicios = new List<Servicio>(),
                HorariosAtencion = new List<HorarioAtencion>(),
                UsuarioId = localDto.UsuarioId
            };

            foreach (var hDto in horariosDto)
            {
                if (!Enum.IsDefined(typeof(DayOfWeek), hDto.DiaSemana)) throw new ArgumentException("Debe seleccionar un día de la semana válido.");

                if (!hDto.EstaCerrado)
                {
                    if (string.IsNullOrEmpty(hDto.HoraApertura)) throw new ArgumentException("El horario de apertura del local es obligatorio.");
                    if (string.IsNullOrEmpty(hDto.HoraCierre)) throw new ArgumentException("El horario de cierre del local es obligatorio.");
                }

                TimeSpan apertura;
                TimeSpan cierre;

                bool aperturaValida = TimeSpan.TryParse(hDto.HoraApertura, out apertura);
                bool cierreValido = TimeSpan.TryParse(hDto.HoraCierre, out cierre);

                if (!hDto.EstaCerrado && (!aperturaValida || !cierreValido)) throw new ArgumentException("El formato del horario no es válido. Debe ser HH:mm.");


                if (!hDto.EstaCerrado && apertura >= cierre) throw new ArgumentException("La hora de apertura no puede ser mayor o igual a la de cierre.");


                local.HorariosAtencion.Add(new HorarioAtencion
                {
                    Id = Guid.NewGuid(),
                    LocalId = local.Id,
                    DiaSemana = hDto.DiaSemana,
                    HoraApertura = apertura,
                    HoraCierre = cierre,
                    EstaCerrado = hDto.EstaCerrado
                });
            }

            await _localRepository.Add(local);

            // Mapeo limpio usando AutoMapper
            var response = _mapper.Map<AddLocalResponseDto>(local);
            response.Message = "Local creado con éxito";

            return response;
        }
    }
 
}
