using AutoMapper;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetMaterialsAuthorisedOnSiteByIdHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetMaterialsAuthorisedOnSiteByIdQuery, MaterialsAuthorisedOnSiteDto>
{
    public async Task<MaterialsAuthorisedOnSiteDto> Handle(GetMaterialsAuthorisedOnSiteByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await rmRepository.GetRegistrationById(request.Id);
        if (registration.Tasks != null)
        {
            registration.Tasks = registration.Tasks
                .Where(t =>
                    t.Task?.Name == RegulatorTaskNames.MaterialsAuthorisedOnSite &&
                    t.Task?.ApplicationTypeId == registration.ApplicationTypeId && !t.Task.IsMaterialSpecific)
                .ToList();
            if (registration.Tasks == null)
            {
                registration.Tasks = new ();
            }
        }
        var materialsAuthorisedDto = mapper.Map<MaterialsAuthorisedOnSiteDto>(registration);
        return materialsAuthorisedDto;
    }
}
