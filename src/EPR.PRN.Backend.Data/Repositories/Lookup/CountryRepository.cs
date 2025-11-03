using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories.Lookup;

public class CountryRepository : ILookupCountryRepository
{
    private readonly EprContext _context;

    public CountryRepository(EprContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LookupCountry>> GetAllAsync()
    {
        return await _context.LookupCountries
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}
