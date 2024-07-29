
namespace EPR.PRN.Backend.API.Repositories.Interfaces
{
    using EPR.PRN.Backend.API.Common.DTO;
    using EPR.PRN.Backend.Data.DataModels;
    using Microsoft.EntityFrameworkCore.Storage;
    using DTO = EPR.PRN.Backend.API.Common.DTO;

    public interface IRepository
    {
        Task<List<EPRN>> GetAllPrnByOrganisationId(Guid orgId);
        Task<EPRN?> GetPrnForOrganisationById(Guid orgId, Guid prnId);
        public IDbContextTransaction BeginTransaction();
        public Task SaveTransaction(IDbContextTransaction transaction);
        //Task UpdateStatus(Guid organisationId, List<PrnUpdateStatusDto> prnUpdates);
    }
}
