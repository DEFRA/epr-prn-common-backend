using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class UpdateOtherPermitsHandler(ICarrierBrokerDealerPermitRepository carrierBrokerDealerPermitRepository)
    : IRequestHandler<UpdateOtherPermitsCommand>
{
    public async Task Handle(UpdateOtherPermitsCommand request, CancellationToken cancellationToken)
    {
        var carrierBrokerDealerPermit = await carrierBrokerDealerPermitRepository.GetByRegistrationId(request.RegistrationId, cancellationToken)
            ?? throw new KeyNotFoundException();

        carrierBrokerDealerPermit.WasteManagementorEnvironmentPermitNumber = request.Dto.WasteLicenseOrPermitNumber;
        carrierBrokerDealerPermit.InstallationPermitorPPCNumber = request.Dto.PpcNumber;
        carrierBrokerDealerPermit.WasteExemptionReference = request.Dto.WasteExemptionReference != null
                ? string.Join(',', request.Dto.WasteExemptionReference)
                : null;
        carrierBrokerDealerPermit.UpdatedBy = request.UserId;
        carrierBrokerDealerPermit.UpdatedOn = DateTime.UtcNow;

        await carrierBrokerDealerPermitRepository.Update(carrierBrokerDealerPermit, cancellationToken);
    }
}