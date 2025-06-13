using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetOtherPermitsHandler(
    ICarrierBrokerDealerPermitRepository carrierBrokerDealerPermitRepository,
    IMapper mapper) : IRequestHandler<GetOtherPermitsQuery, GetOtherPermitsResultDto>
{
    public async Task<GetOtherPermitsResultDto?> Handle(GetOtherPermitsQuery request, CancellationToken cancellationToken)
    {
        var model = await carrierBrokerDealerPermitRepository.GetByRegistrationId(request.RegistrationId, cancellationToken);

        return model != null ? mapper.Map<GetOtherPermitsResultDto>(model) : null;
    }
}
