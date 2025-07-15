using EPR.PRN.Backend.Data.DTO;
using MediatR;

namespace EPR.PRN.Backend.API.Queries;

public class GetOverseasMaterialReprocessingSitesQuery : IRequest<IList<OverseasMaterialReprocessingSiteDto>>
{
    public required Guid RegistrationMaterialId { get; set; }
}