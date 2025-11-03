using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Queries;

/// <summary>
/// Query to retrieve a list of material permit types.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class GetMaterialsPermitTypesQuery : IRequest<List<MaterialsPermitTypeDto>>
{
}
