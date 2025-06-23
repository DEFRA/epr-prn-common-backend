using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Dto;
using MediatR;

namespace EPR.PRN.Backend.API.Queries;

[ExcludeFromCodeCoverage]
public class GetAllRegistrationMaterialsQuery : IRequest<IList<ApplicantRegistrationMaterialDto>>
{
    public required Guid RegistrationId { get; set; }
}