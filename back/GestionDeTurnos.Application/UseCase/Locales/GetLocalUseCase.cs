using GestionDeTurnos.Application.Interface;
using GestionDeTurnos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTurnos.Application.UseCase.Locales
{
    public class GetLocalUseCase
    {
        private readonly ILocalRepository _localRepository;

            public GetLocalUseCase(ILocalRepository localRepository)
        {
            _localRepository = localRepository;
        }
        public List<Local> GetLocal()
        {
            List<Local> locales = _localRepository.GetAll();
            return locales;
        }
    }
}
