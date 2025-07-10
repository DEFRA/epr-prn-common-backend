using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.Data.DTO;
using MediatR;

namespace EPR.PRN.Backend.API.Queries;

[ExcludeFromCodeCoverage]
public class GetOverseasMaterialReprocessingSitesQuery : IRequest<IList<OverseasMaterialReprocessingSiteDto>>
{
    public required Guid RegistrationMaterialId { get; set; }
}