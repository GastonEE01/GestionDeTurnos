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

        public AddLocalUseCase(ILocalRepository localRepository,IHorarioAtencionRepository horarioAtencion)
        {
            _localRepository = localRepository;
            _horarioAtencionRepository = horarioAtencion;
        }

        public Local AddLocal(LocalRequestDto localDto, List<HorarioAtencionRequestDto> horariosDto)
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
            _localRepository.GetAll().ToList().ForEach(local =>
            {
                if (local.Name.Equals(localDto.Name, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Ya existe un local con el mismo nombre.");
                }
            });

            // Mejorar que los locales puedan mostrar mas horatios turnos
            
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
