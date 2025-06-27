using EPR.PRN.Backend.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.API.Queries;

public class GetCountriesHandler : IRequestHandler<GetCountriesQuery, IEnumerable<string>>
{
    private readonly ILookupCountryRepository _repository;

    public GetCountriesHandler(ILookupCountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<string>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = await _repository.GetAllAsync();
        return countries.Select(c => c.Name);
    }
}
