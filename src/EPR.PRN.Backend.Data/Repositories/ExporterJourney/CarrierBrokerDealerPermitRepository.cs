using EPR.PRN.Backend.Data.DataModels.ExporterJourney;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.Repositories.ExporterJourney;

[ExcludeFromCodeCoverage]
public class CarrierBrokerDealerPermitRepository(EprContext context) : ICarrierBrokerDealerPermitRepository
{
    public async Task<CarrierBrokerDealerPermit?> GetByRegistrationId(Guid registrationId, CancellationToken cancellationToken)
    {
        return await context.CarrierBrokerDealerPermits
            .AsNoTracking()
            .Include(x => x.Registration)
            .Where(x => x.Registration.ExternalId == registrationId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task Add(CarrierBrokerDealerPermit entity, CancellationToken cancellationToken)
    {
        context.CarrierBrokerDealerPermits.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(CarrierBrokerDealerPermit entity, CancellationToken cancellationToken)
    {
        context.CarrierBrokerDealerPermits.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
