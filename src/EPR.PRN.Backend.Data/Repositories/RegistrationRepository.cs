using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class RegistrationRepository(EprRegistrationsContext context) : IRegistrationRepository
{
    public async Task UpdateSiteAddress(int registrationId, AddressDto reprocessingSiteAddress, AddressDto legalDocumentAddress)
    {
        var registration = await context.Registrations.FirstOrDefaultAsync(x => x.Id == registrationId);

        if (registration is null)
        {
            throw new KeyNotFoundException("Registration not found.");
        }

        // Reprocessing Site Address
        if (reprocessingSiteAddress.Id.GetValueOrDefault() == 0)
        {
            var address = new LookupAddress
            {
                AddressLine1 = reprocessingSiteAddress.AddressLine1,
                AddressLine2 = reprocessingSiteAddress.AddressLine2,
                TownCity = reprocessingSiteAddress.TownCity,
                County = reprocessingSiteAddress.County,
                Country = reprocessingSiteAddress.Country,
                PostCode = reprocessingSiteAddress.PostCode,
                
                NationId = reprocessingSiteAddress.NationId,
                
                GridReference = reprocessingSiteAddress.GridReference
            };

            await context.LookupAddresses.AddAsync(address);

            reprocessingSiteAddress.Id = address.Id;
        }

        // Legal Document Address
        if (legalDocumentAddress.Id.GetValueOrDefault() == 0)
        {
            var address = new LookupAddress
            {
                AddressLine1 = legalDocumentAddress.AddressLine1,
                AddressLine2 = legalDocumentAddress.AddressLine2,
                TownCity = legalDocumentAddress.TownCity,
                County = legalDocumentAddress.County,
                Country = legalDocumentAddress.Country,
                PostCode = legalDocumentAddress.PostCode,

                NationId = legalDocumentAddress.NationId,
                
                GridReference = legalDocumentAddress.GridReference
            };

            await context.LookupAddresses.AddAsync(address);

            legalDocumentAddress.Id = address.Id;
        }

        registration.ReprocessingSiteAddressId = reprocessingSiteAddress.Id;
        registration.LegalDocumentAddressId = legalDocumentAddress.Id;

        await context.SaveChangesAsync();
    }
}
