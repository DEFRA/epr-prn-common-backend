using MediatR;
using System.Collections.Generic;

public record GetCountriesQuery : IRequest<IEnumerable<string>>;
