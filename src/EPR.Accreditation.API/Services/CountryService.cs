using EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Repositories.Interfaces;
using EPR.Accreditation.API.Services.Interfaces;

namespace EPR.Accreditation.API.Services
{
    public class CountryService : ICountryService
    {
        protected readonly IRepository _repository;

        public CountryService(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<Country>> GetCountryList()
        {
            return await _repository.GetCountries();    
        }
    }
}
