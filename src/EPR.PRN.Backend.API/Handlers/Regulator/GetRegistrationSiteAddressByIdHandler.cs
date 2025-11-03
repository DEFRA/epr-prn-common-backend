using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetRegistrationSiteAddressByIdHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetRegistrationSiteAddressByIdQuery, RegistrationSiteAddressDto>
{
    public async Task<RegistrationSiteAddressDto> Handle(GetRegistrationSiteAddressByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await rmRepository.GetRegistrationById(request.Id);
        var siteAddressDto = registration.ReprocessingSiteAddress != null ? mapper.Map<RegistrationSiteAddressDto>(registration) : new RegistrationSiteAddressDto();

        return siteAddressDto;
    }
}