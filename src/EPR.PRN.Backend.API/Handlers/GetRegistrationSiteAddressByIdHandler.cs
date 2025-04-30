using AutoMapper;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class GetRegistrationSiteAddressByIdHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetRegistrationSiteAddressByIdQuery, RegistrationSiteAddressDto>
{
    public async Task<RegistrationSiteAddressDto> Handle(GetRegistrationSiteAddressByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await rmRepository.GetRegistrationById(request.Id);


        var siteAddressDto = registration.ReprocessingSiteAddressId != null ? mapper.Map<RegistrationSiteAddressDto>(registration) : new RegistrationSiteAddressDto();
        return siteAddressDto;
    }
}