using EPR.PRN.Backend.Data.DataModels.ExporterJourney;

namespace EPR.PRN.Backend.Data.Interfaces;

public interface ICarrierBrokerDealerPermitRepository
{
    Task<CarrierBrokerDealerPermit?> GetByRegistrationId(Guid registrationId, CancellationToken cancellationToken);
    Task Add(CarrierBrokerDealerPermit entity, CancellationToken cancellationToken);
    Task Update(CarrierBrokerDealerPermit entity, CancellationToken cancellationToken);
}
