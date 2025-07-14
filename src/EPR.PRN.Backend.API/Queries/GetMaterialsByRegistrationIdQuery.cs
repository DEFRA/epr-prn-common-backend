using System.Diagnostics.CodeAnalysis;
using MediatR;
using MaterialDto = EPR.PRN.Backend.API.Dto.MaterialDto;

namespace EPR.PRN.Backend.API.Queries;

[ExcludeFromCodeCoverage]
public class GetMaterialsByRegistrationIdQuery : IRequest<IList<MaterialDto>>
{
    public Guid RegistrationId { get; set; }
}