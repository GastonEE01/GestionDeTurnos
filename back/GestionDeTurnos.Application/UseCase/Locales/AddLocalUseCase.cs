using AutoMapper;
using GestionDeTurnos.Application.DTOs; // ajustar al namespace real donde esté LocalRequestDto
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
        public AddLocalUseCase(ILocalRepository localRepository,IHorarioAtencionRepository horarioAtencionRepository, IUserRepository userRepository)
        {
            _localRepository = localRepository;
            _horarioAtencionRepository = horarioAtencionRepository;
            _userRepository = userRepository;
        }

        public async Task<Local> AddLocal(LocalRequestDto localDto, List<HorarioAtencionRequestDto> horariosDto)
        {
            if(localDto == null)
            {
                throw new ArgumentNullException("No se pudo crear el local");
            }

            // Validar que los campos obligatorios no estén vacíos
            if (string.IsNullOrEmpty(localDto.Name))
            {
                throw new ArgumentNullException("El nombre del local es obligatorio.");
            }

            // Validar que no exista un local con el mismo nombre
            var locales = await _localRepository.GetAll();
            locales.ForEach(local =>
            {
                if (local.Name.Equals(localDto.Name, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Ya existe un local con el mismo nombre.");
                }
            });

            // Validar que el usuario exista y tenga permisos para crear un local
            var usuario = await _userRepository.GetUsuarioByIdAsync(localDto.UsuarioId);
            if (usuario == null)
            {
                throw new InvalidOperationException("El usuario no existe.");
            }
            if(usuario.Rol == "Admin" && usuario.Rol != "Cliente")
            {
                throw new InvalidOperationException("El usuario no tiene permisos para crear un local.");
            }


            var local = new Local
            {
                Id = Guid.NewGuid(),
                Name = localDto.Name,
                Description = localDto.Description,
                Category = localDto.Category,
                ImageURL = localDto.ImageURL,
                Title = localDto.Title,
                Direction = localDto.Direction,
                Phone = localDto.Phone,
                Servicios = new List<Servicio>(),
                HorariosAtencion = new List<HorarioAtencion>(),
                UsuarioId = localDto.UsuarioId
            };

            // Crear tabla de horarios para el local
            // Validar que los campos obligatorios no estén vacíos
            if(horariosDto == null || horariosDto.Count == 0)
            {
                throw new ArgumentNullException("No se proporcionaron horarios de atención válidos.");
            }

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

                // Agregamos el horario directamente a la lista del local 
                local.HorariosAtencion.Add(horarios);
            }
            _localRepository.Add(local);
            // Agregar el horario de atención al local
            /*  List<HorarioAtencion> horarioLocal = _mapper.Map<List<HorarioAtencion>>(horarioDto);
              local.HorariosAtencion = horarioLocal;

              
              _horarioAtencionRepository.Add(horarios);*/
            return local;
        }

       

        /*  public object AddLocal(LocalRequestDto local, List<HorarioAtencionRequestDto> horarios)
          {
              throw new NotImplementedException();
          }*/
    }
 
}
