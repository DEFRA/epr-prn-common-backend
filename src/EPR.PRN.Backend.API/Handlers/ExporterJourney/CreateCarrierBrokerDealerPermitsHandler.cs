using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.ExporterJourney;

public class CreateCarrierBrokerDealerPermitsHandler(
    ICarrierBrokerDealerPermitRepository carrierBrokerDealerPermitRepository,
    IRegistrationRepository registrationRepository)
    : IRequestHandler<CreateCarrierBrokerDealerPermitsCommand, bool>
{
    public async Task<bool> Handle(CreateCarrierBrokerDealerPermitsCommand request, CancellationToken cancellationToken)
    {
        var registration = await registrationRepository.GetRegistrationByExternalId(request.RegistrationId, cancellationToken)
            ?? throw new KeyNotFoundException();

        if (registration.CarrierBrokerDealerPermit != null)
        {
            return false;
        }

        var carrierBrokerDealerPermit = new CarrierBrokerDealerPermits
        {
            RegistrationId = registration.Id, 
            WasteCarrierBrokerDealerRegistration = request.WasteCarrierBrokerDealerRegistration,
            CreatedBy = request.UserId,
            CreatedDate = DateTime.UtcNow
        };

        await carrierBrokerDealerPermitRepository.Add(carrierBrokerDealerPermit, cancellationToken);

        return true;
    }
}