using System.Collections.Generic;
using System.Threading.Tasks;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces;

public interface ILookupCountryRepository
{
    Task<IEnumerable<LookupCountry>> GetAllAsync();
}
