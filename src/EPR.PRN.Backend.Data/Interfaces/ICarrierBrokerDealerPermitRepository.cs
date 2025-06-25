using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces;

public interface ICarrierBrokerDealerPermitRepository
{
    Task<CarrierBrokerDealerPermits?> GetByRegistrationId(Guid registrationId, CancellationToken cancellationToken);
    Task Add(CarrierBrokerDealerPermits entity, CancellationToken cancellationToken);
    Task Update(CarrierBrokerDealerPermits entity, CancellationToken cancellationToken);
}
