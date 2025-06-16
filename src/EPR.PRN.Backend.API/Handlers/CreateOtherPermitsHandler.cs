using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class CreateOtherPermitsHandler(
    ICarrierBrokerDealerPermitRepository carrierBrokerDealerPermitRepository,
    IRegistrationRepository registrationRepository)
    : IRequestHandler<CreateOtherPermitsCommand, bool>
{
    public async Task<bool> Handle(CreateOtherPermitsCommand request, CancellationToken cancellationToken)
    {
        var registration = await registrationRepository.GetRegistrationByExternalId(request.RegistrationId, cancellationToken)
            ?? throw new KeyNotFoundException();

        if (registration.CarrierBrokerDealerPermit != null)
        {
            return false;
        }

        var carrierBrokerDealerPermit = new CarrierBrokerDealerPermit
        {
            RegistrationId = registration.Id,
            WasteManagementorEnvironmentPermitNumber = request.Dto.WasteLicenseOrPermitNumber,
            InstallationPermitorPPCNumber = request.Dto.PpcNumber,
            WasteExemptionReference = request.Dto.WasteExemptionReference != null
                ? string.Join(',', request.Dto.WasteExemptionReference)
                : null,
            CreatedBy = request.UserId,
            CreatedOn = DateTime.UtcNow
        };

        await carrierBrokerDealerPermitRepository.Add(carrierBrokerDealerPermit, cancellationToken);

        return true;
    }
}