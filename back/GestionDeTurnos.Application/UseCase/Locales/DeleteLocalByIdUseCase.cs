using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Locales
{
    public class DeleteLocalByIdUseCase
    {
        private readonly ILocalRepository _localRepository;

        public DeleteLocalByIdUseCase(ILocalRepository localRepository)
        {
            _localRepository = localRepository;
        }

        public async Task DeleteLocal(Guid id)
        {
            Local searchLocal = await _localRepository.GetLocalById(id);
            if (searchLocal == null)
            {
                throw new ArgumentException("No se encontro el local para eliminar");
            }
            await _localRepository.DeleteLocal(searchLocal);
        }
    }
}
