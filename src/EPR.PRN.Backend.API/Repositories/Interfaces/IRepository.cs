
namespace EPR.PRN.Backend.API.Repositories.Interfaces
{
    using EPR.PRN.Backend.API.Common.DTO;
    using EPR.PRN.Backend.Data.DataModels;
    using Microsoft.EntityFrameworkCore.Storage;
    using DTO = EPR.PRN.Backend.API.Common.DTO;

    public interface IRepository
    {
        Task<List<Eprn>> GetAllPrnByOrganisationId(Guid orgId);
        Task<Eprn?> GetPrnForOrganisationById(Guid orgId, Guid prnId);
        public IDbContextTransaction BeginTransaction();
        public Task SaveTransaction(IDbContextTransaction transaction);
        void AddPrnStatusHistory(PrnStatusHistory prnStatusHistory);
    }
}
