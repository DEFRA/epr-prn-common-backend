namespace EPR.PRN.Backend.API.Services.Interfaces
{
    using EPR.PRN.Backend.API.Common.DTO;

    public interface IPrnService
    {
        Task<PrnDTo?> GetPrnForOrganisationById(Guid orgId, Guid prnId);

        Task<List<PrnDTo>> GetAllPrnByOrganisationId(Guid orgId);
        Task UpdateStatus(Guid orgId, List<PrnUpdateStatusDto> prnUpdates);
    }
}
