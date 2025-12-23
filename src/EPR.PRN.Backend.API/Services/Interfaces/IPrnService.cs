using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Models;
using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.API.Services.Interfaces;

public interface IPrnService
{
    Task<PrnDto?> GetPrnForOrganisationById(Guid orgId, Guid prnId);
    Task<List<PrnDto>> GetAllPrnByOrganisationId(Guid orgId);
    Task UpdateStatus(Guid orgId, Guid userId, List<PrnUpdateStatusDto> prnUpdates);
    Task<PaginatedResponseDto<PrnDto>> GetSearchPrnsForOrganisation(
        Guid orgId,
        PaginatedRequestDto request
    );
    Task<List<PrnUpdateStatus>?> GetModifiedPrnsbyDate(DateTime fromDate, DateTime toDate);
    Task<Eprn> SaveEprnDetails(Eprn prn);

    #region Npwd specific
    Task InsertPeprNpwdSyncPrns(List<InsertSyncedPrn> syncedPrns);
    Task<List<PrnStatusSync>?> GetNpwdSyncStatuses(DateTime fromDate, DateTime toDate);
    Task<List<NpwdPrnUpdateStatus>?> GetModifiedNpwdPrnsbyDate(DateTime fromDate, DateTime toDate);
    Task SaveNpwdPrnDetails(SaveNpwdPrnDetailsRequest prn);

    #endregion
}
