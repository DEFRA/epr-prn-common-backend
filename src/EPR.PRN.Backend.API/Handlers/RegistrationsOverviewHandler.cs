using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class RegistrationsOverviewHandler(
    IRegistrationRepository rmRepository
) : IRequestHandler<RegistrationsOverviewCommand>
{
    public async Task<List<RegistrationDto>> Handle(RegistrationsOverviewCommand request, CancellationToken cancellationToken)
    {
        var registrations = await rmRepository.GetRegistrationsForOrgAsync(request.OrganisationId);

        return await Task.FromResult(new List<RegistrationDto>());
    }   
}
