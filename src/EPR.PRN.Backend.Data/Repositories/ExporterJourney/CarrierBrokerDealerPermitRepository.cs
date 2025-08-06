using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.Repositories.ExporterJourney;

[ExcludeFromCodeCoverage]
public class CarrierBrokerDealerPermitRepository(EprContext context) : ICarrierBrokerDealerPermitRepository
{
    public async Task<CarrierBrokerDealerPermits?> GetByRegistrationId(Guid registrationId, CancellationToken cancellationToken)
    {
        return await context.CarrierBrokerDealerPermits
            .Include(x => x.Registration)
            .AsNoTracking()
            .Where(x => x.Registration.ExternalId == registrationId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task Add(CarrierBrokerDealerPermits entity, CancellationToken cancellationToken)
    {
        context.CarrierBrokerDealerPermits.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(CarrierBrokerDealerPermits entity, CancellationToken cancellationToken)
    {
        context.CarrierBrokerDealerPermits.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
