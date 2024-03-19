using EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Services.Interfaces
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetCountryList();
    }
}
