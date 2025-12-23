namespace EPR.PRN.Backend.API.Repositories.Interfaces
{
    using EPR.PRN.Backend.API.Common.Dto;
    using EPR.PRN.Backend.API.Dto;
    using EPR.PRN.Backend.Data.DataModels;
    using Microsoft.EntityFrameworkCore.Storage;

    public interface IRepository
    {
        Task<List<Eprn>> GetAllPrnByOrganisationId(Guid orgId);
        Task<Eprn?> GetPrnForOrganisationById(Guid orgId, Guid prnId);
        public IDbContextTransaction BeginTransaction();
        public Task SaveTransaction(IDbContextTransaction transaction);
        void AddPrnStatusHistory(PrnStatusHistory prnStatusHistory);
        Task<PaginatedResponseDto<PrnDto>> GetSearchPrnsForOrganisation(
            Guid orgId,
            PaginatedRequestDto request
        );
        Task<List<PrnUpdateStatus>> GetModifiedPrnsbyDate(DateTime fromDate, DateTime toDate);
        Task<Eprn> SavePrnDetails(Eprn entity);
        Task<List<Eprn>> GetPrnsForPrnNumbers(List<string> prnNumbers);

        #region Npwd Specified
        Task<List<NpwdPrnUpdateStatus>> GetModifiedNpwdPrnsbyDate(
            DateTime fromDate,
            DateTime toDate
        );
        Task<List<PrnStatusSync>> GetNpwdSyncStatuses(DateTime fromDate, DateTime toDate);
        Task InsertPeprNpwdSyncPrns(List<PEprNpwdSync> syncedPrns);
        #endregion
    }
}
