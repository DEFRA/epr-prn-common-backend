using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.ExporterJourney;

public class UpdateCarrierBrokerDealerPermitsHandler(ICarrierBrokerDealerPermitRepository carrierBrokerDealerPermitRepository)
    : IRequestHandler<UpdateCarrierBrokerDealerPermitsCommand>
{
    public async Task Handle(UpdateCarrierBrokerDealerPermitsCommand request, CancellationToken cancellationToken)
    {
        var carrierBrokerDealerPermit = await carrierBrokerDealerPermitRepository.GetByRegistrationId(request.RegistrationId, cancellationToken)
            ?? throw new KeyNotFoundException();

        if (!string.IsNullOrWhiteSpace(request.Dto.WasteCarrierBrokerDealerRegistration))
        {
            carrierBrokerDealerPermit.WasteCarrierBrokerDealerRegistration = request.Dto.WasteCarrierBrokerDealerRegistration;
        }
        else
        {
            carrierBrokerDealerPermit.WasteManagementEnvironmentPermitNumber = request.Dto.WasteLicenseOrPermitNumber;
            carrierBrokerDealerPermit.InstallationPermitOrPPCNumber = request.Dto.PpcNumber;

            if (request.Dto.WasteExemptionReference != null)
            {
                carrierBrokerDealerPermit.WasteExemptionReference = string.Join(',', request.Dto.WasteExemptionReference);
            }
        }


        carrierBrokerDealerPermit.UpdatedBy = request.UserId;
        carrierBrokerDealerPermit.UpdatedDate = DateTime.UtcNow;

        await carrierBrokerDealerPermitRepository.Update(carrierBrokerDealerPermit, cancellationToken);
    }
}