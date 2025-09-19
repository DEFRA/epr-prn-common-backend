using MediatR;

namespace EPR.PRN.Backend.API.Queries
{
    public readonly record struct GetCountriesQuery() : IRequest<IEnumerable<string>>;
}