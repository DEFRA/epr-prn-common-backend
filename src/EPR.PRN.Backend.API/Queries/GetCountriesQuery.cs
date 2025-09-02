using MediatR;

namespace EPR.PRN.Backend.API.Queries
{
    public record GetCountriesQuery() : IRequest<IEnumerable<string>>;
}