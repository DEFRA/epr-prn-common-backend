using System.Diagnostics.CodeAnalysis;
using MediatR;
using MaterialDto = EPR.PRN.Backend.API.Dto.MaterialDto;

namespace EPR.PRN.Backend.API.Queries;

[ExcludeFromCodeCoverage]
public class GetAllMaterialsQuery : IRequest<IList<MaterialDto>>
{
}