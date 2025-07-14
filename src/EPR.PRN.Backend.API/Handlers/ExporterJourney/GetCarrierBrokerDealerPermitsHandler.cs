using AutoMapper;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.API.Queries.ExporterJourney;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.ExporterJourney;

public class GetCarrierBrokerDealerPermitsHandler(
    ICarrierBrokerDealerPermitRepository carrierBrokerDealerPermitRepository,
    IMapper mapper) : IRequestHandler<CarrierBrokerDealerPermitsQuery, GetCarrierBrokerDealerPermitsResultDto?>
{
    public async Task<GetCarrierBrokerDealerPermitsResultDto?> Handle(CarrierBrokerDealerPermitsQuery request, CancellationToken cancellationToken)
    {
        var model = await carrierBrokerDealerPermitRepository.GetByRegistrationId(request.RegistrationId, cancellationToken);

        return model != null ? mapper.Map<GetCarrierBrokerDealerPermitsResultDto>(model) : null;
    }
}
