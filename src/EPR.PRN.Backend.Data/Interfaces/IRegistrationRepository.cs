using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IRegistrationRepository
    {
        Task UpdateSiteAddress(int registrationId, AddressDto reprocessingSiteAddress, AddressDto legalDocumentAddress);
    }
}
