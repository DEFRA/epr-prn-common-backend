using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;
using RegistrationOverviewDto = EPR.PRN.Backend.Data.DTO.Registration.RegistrationOverviewDto;

namespace EPR.PRN.Backend.API.Handlers;

public class GetRegistrationsOverviewByOrgIdHandler(IRegistrationRepository repo) 
    : IRequestHandler<GetRegistrationsOverviewByOrgIdQuery, IEnumerable<RegistrationOverviewDto>>
{
    public async Task<IEnumerable<RegistrationOverviewDto>> Handle(GetRegistrationsOverviewByOrgIdQuery request, CancellationToken cancellationToken)
    {
        return await repo.GetRegistrationsOverviewForOrgIdAsync(request.OrganisationId);
    }
}