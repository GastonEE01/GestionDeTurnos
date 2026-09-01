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

        /* public async Task<AddLocalResponseDto> AddLocal(LocalRequestDto localDto, List<HorarioAtencionRequestDto> horariosDto)
         {

             if(localDto == null || horariosDto == null) throw new ArgumentNullException("No se pudo crear el local");

             if (string.IsNullOrEmpty(localDto.Name)) throw new ArgumentNullException("El nombre del local es obligatorio.");

             if (string.IsNullOrEmpty(localDto.Description)) throw new ArgumentNullException("La descripcion del local es obligatorio.");

             if (string.IsNullOrEmpty(localDto.Category)) throw new ArgumentNullException("La categoria del local es obligatorio.");

             if (string.IsNullOrEmpty(localDto.Direction)) throw new ArgumentNullException("La direccion del local es obligatorio.");

             if (string.IsNullOrEmpty(localDto.Phone)) throw new ArgumentNullException("El telefono del local es obligatorio.");

             // Validar que no exista un local con el mismo nombre
             var locales = await _localRepository.GetAll();

             if (locales.Any(l => l.Name.Equals(localDto.Name, StringComparison.OrdinalIgnoreCase)))  throw new InvalidOperationException("Ya existe un local con el mismo nombre.");

             if (localDto.UsuarioId == Guid.Empty) throw new InvalidOperationException("El local debe estar asociado a un usuario válido.");

             locales.ForEach(local =>
             {
                 if (local.Name.Equals(localDto.Name, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("Ya existe un local con el mismo nombre.");
             });

             // Validar que el usuario exista y tenga permisos para crear un local
             var usuario = await _userRepository.GetUsuarioByIdAsync(localDto.UsuarioId);
             if (usuario == null) throw new InvalidOperationException("El usuario no existe.");

             if(usuario.Rol == "Cliente")  throw new InvalidOperationException("El usuario no tiene permisos para crear un local.");

             var local = new Local
             {
                 Id = Guid.NewGuid(),
                 Name = localDto.Name,
                 Description = localDto.Description,
                 Category = localDto.Category,
                 ImageURL = localDto.ImageURL,
                 //Title = localDto.Title,
                 Direction = localDto.Direction,
                 Phone = localDto.Phone,
                 Servicios = new List<Servicio>(),
                 HorariosAtencion = new List<HorarioAtencion>(),
                 UsuarioId = localDto.UsuarioId
             };

             // Crear tabla de horarios para el local
             // Validar que los campos obligatorios no estén vacíos
             if(horariosDto == null || horariosDto.Count == 0) throw new ArgumentNullException("No se proporcionaron horarios de atención válidos.");

             foreach (var hDto in horariosDto)
             {
                 var horarios = new HorarioAtencion
                 {
                     Id = Guid.NewGuid(),
                     LocalId = local.Id,
                     DiaSemana = hDto.DiaSemana,
                     HoraApertura = hDto.HoraApertura,
                     HoraCierre = hDto.HoraCierre,
                     EstaCerrado = hDto.EstaCerrado
                 };

                 local.HorariosAtencion.Add(horarios);
             }

             _localRepository.Add(local);

             AddLocalResponseDto response = new AddLocalResponseDto
             {
                 Id = local.Id,
                 Name = local.Name,
                 Description = local.Description,
                 Category = local.Category,
                 ImageURL = local.ImageURL,
                 Direction = local.Direction,
                 Phone = local.Phone,
                 Servicios = new List<GetServicioByLocalResponseDto>(),
                 HorariosAtencion = new List<HorarioAtencionRequestDto>(),
                 Message = "Local creado"
             };

             return _mapper.Map<AddLocalResponseDto>(local);
         }
 */
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

            // Validar que no exista un local con el mismo nombre
            var locales = await _localRepository.GetAll();
            if (locales.Any(l => l.Name.Equals(localDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Ya existe un local con el mismo nombre.");
            }

            // Validar que el ID de usuario no venga vacío
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
