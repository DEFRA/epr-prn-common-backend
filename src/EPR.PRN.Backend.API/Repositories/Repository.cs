namespace EPR.PRN.Backend.API.Repositories;

using System.Collections.Generic;
using System.Linq.Expressions;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using static EPR.PRN.Backend.API.Common.Constants.PrnConstants;

public class Repository(
    EprContext eprContext,
    ILogger<Repository> logger,
    IConfiguration configuration
) : IRepository
{
    protected readonly EprContext _eprContext = eprContext;
    private readonly string? logPrefix = string.IsNullOrEmpty(configuration["LogPrefix"])
        ? "[EPR.PRN.Backend]"
        : configuration["LogPrefix"];

    private IQueryable<Eprn> GetAllPrnsForOrganisation(Guid orgId)
    {
        logger.LogInformation(
            "{Logprefix}: Repository - GetAllPrnsForOrganisation: request for organisation {Organisation}",
            logPrefix,
            orgId
        );
        var prns = _eprContext.Prn.Where(x => x.OrganisationId == orgId);
        logger.LogInformation(
            "{Logprefix}: Repository - GetAllPrnsForOrganisation: PRN query prepared for organisation {Organisation}",
            logPrefix,
            orgId
        );
        return prns;
    }

    public async Task<List<Eprn>> GetAllPrnByOrganisationId(Guid orgId)
    {
        logger.LogInformation(
            "{Logprefix}: Repository - GetAllPrnByOrganisationId: request for organisation {Organisation}",
            logPrefix,
            orgId
        );
        var prns = await GetAllPrnsForOrganisation(orgId).ToListAsync();
        logger.LogInformation(
            "{Logprefix}: Repository - GetAllPrnByOrganisationId: Prns fetched {Prns}",
            logPrefix,
            JsonConvert.SerializeObject(prns)
        );
        return prns;
    }

    public async Task<List<NpwdPrnUpdateStatus>> GetModifiedNpwdPrnsbyDate(
        DateTime fromDate,
        DateTime toDate
    )
    {
        var result = await (
            from p in _eprContext.Prn
            join ps in _eprContext.PrnStatus on p.PrnStatusId equals ps.Id
            where
                p.StatusUpdatedOn >= fromDate
                && p.StatusUpdatedOn <= toDate
                && p.SourceSystemId == null
                && (p.PrnStatusId == 1 || p.PrnStatusId == 2)
                && !_eprContext.PEprNpwdSync.Any(s =>
                    p.Id == s.PRNId && p.PrnStatusId == s.PRNStatusId
                )
            select new
            {
                p.PrnNumber,
                ps.StatusName,
                p.StatusUpdatedOn,
                p.AccreditationYear,
                p.ObligationYear
            }
        ).ToListAsync();

        var prnUpdateStatuses = result
            .Select(p => new NpwdPrnUpdateStatus
            {
                EvidenceNo = p.PrnNumber,
                EvidenceStatusCode = MapStatusCode(
                    (EprnStatus)Enum.Parse(typeof(EprnStatus), p.StatusName)
                ),
                StatusDate = p.StatusUpdatedOn?.ToUniversalTime(),
                AccreditationYear = p.AccreditationYear,
                ObligationYear = p.ObligationYear,
            })
            .ToList();

        return prnUpdateStatuses;
    }

    public async Task<List<PrnUpdateStatus>> GetModifiedPrnsbyDate(
        DateTime fromDate,
        DateTime toDate
    )
    {
        var result = await (
            from p in _eprContext.Prn
            where
                p.StatusUpdatedOn >= fromDate
                && p.StatusUpdatedOn <= toDate
                && p.SourceSystemId != null
                && (p.PrnStatusId == 1 || p.PrnStatusId == 2)
            select new
            {
                p.PrnNumber,
                p.PrnStatusId,
                p.StatusUpdatedOn,
                p.AccreditationYear,
                p.SourceSystemId,
                p.ObligationYear
            }
        ).ToListAsync();

        var prnUpdateStatuses = result
            .Select(p => new PrnUpdateStatus
            {
                PrnNumber = p.PrnNumber,
                PrnStatusId = p.PrnStatusId,
                StatusDate = p.StatusUpdatedOn?.ToUniversalTime(),
                AccreditationYear = p.AccreditationYear,
                SourceSystemId = p.SourceSystemId,
                ObligationYear = p.ObligationYear,
            })
            .ToList();

        return prnUpdateStatuses;
    }

    public async Task<List<PrnStatusSync>> GetNpwdSyncStatuses(DateTime fromDate, DateTime toDate)
    {
        var result = await (
            from p in _eprContext.Prn
            join ps in _eprContext.PEprNpwdSync on p.Id equals ps.PRNId
            where p.SourceSystemId == null
            where ps.CreatedOn >= fromDate && ps.CreatedOn < toDate
            select new
            {
                p.PrnNumber,
                ps.PRNStatusId,
                p.OrganisationName,
                ps.CreatedOn,
            }
        ).ToListAsync();

        var prnStatusSync = result
            .Select(p => new PrnStatusSync
            {
                PrnNumber = p.PrnNumber,
                StatusName = MapStatusCode((EprnStatus)p.PRNStatusId),
                OrganisationName = p.OrganisationName,
                UpdatedOn = p.CreatedOn,
            })
            .ToList();

        return prnStatusSync;
    }

    public async Task<Eprn?> GetPrnForOrganisationById(Guid orgId, Guid prnId)
    {
        logger.LogInformation(
            "{Logprefix}: Repository - GetPrnForOrganisationById: request for organisation {Organisation}, Prn {PrnId}",
            logPrefix,
            orgId,
            prnId
        );
        var eprn = await GetAllPrnsForOrganisation(orgId)
            .FirstOrDefaultAsync(p => p.ExternalId == prnId);
        logger.LogInformation(
            "{Logprefix}: Repository - GetPrnForOrganisationById: Eprn fetched {Eprn}",
            logPrefix,
            JsonConvert.SerializeObject(eprn)
        );
        return eprn;
    }

    public async Task SaveTransaction(IDbContextTransaction transaction)
    {
        logger.LogInformation(
            "{Logprefix}: Repository - SaveTransaction: Saving Transaction",
            logPrefix
        );
        await _eprContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public IDbContextTransaction BeginTransaction()
    {
        return _eprContext.Database.BeginTransaction();
    }

    public void AddPrnStatusHistory(PrnStatusHistory prnStatusHistory)
    {
        logger.LogInformation(
            "{Logprefix}: Repository - AddPrnStatusHistory: {PrnStatusHistory}",
            logPrefix,
            JsonConvert.SerializeObject(prnStatusHistory)
        );
        _eprContext.PrnStatusHistory.Add(prnStatusHistory);
    }

    private static string MapStatusCode(EprnStatus status)
    {
        string result = string.Empty;
        if (status == EprnStatus.ACCEPTED)
        {
            result = EvidenceStatusCode.EV_ACCEP.ToHyphenatedString();
        }
        else if (status == EprnStatus.REJECTED)
        {
            result = EvidenceStatusCode.EV_ACANCEL.ToHyphenatedString();
        }
        return result;
    }

    private static Expression<Func<Eprn, bool>> GetFilterByCondition(string? filterBy)
    {
        return filterBy switch
        {
            Filters.AcceptedAll => p => p.PrnStatusId == (int)EprnStatus.ACCEPTED,

            Filters.CancelledAll => p => p.PrnStatusId == (int)EprnStatus.CANCELLED,

            Filters.RejectedAll => p => p.PrnStatusId == (int)EprnStatus.REJECTED,

            Filters.AwaitingAll => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE,

            Filters.AwaitingAluminium => p =>
                p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                && p.MaterialName == Materials.Aluminium,

            Filters.AwaitingGlassOther => p =>
                p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                && p.MaterialName == Materials.GlassOther,

            Filters.AwaitingGlassMelt => p =>
                p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                && p.MaterialName == Materials.GlassMelt,

            Filters.AwaitngPaperFiber => p =>
                p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                && (p.MaterialName == Materials.PaperFiber || p.MaterialName == Materials.Fibre),

            Filters.AwaitngPlastic => p =>
                p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                && p.MaterialName == Materials.Plastic,

            Filters.AwaitngSteel => p =>
                p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                && p.MaterialName == Materials.Steel,

            Filters.AwaitngWood => p =>
                p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                && p.MaterialName == Materials.Wood,

            _ => sm => true,
        };
    }

    private static (string order, Expression<Func<Eprn, dynamic>> expr) GetOrderByCondition(
        string? sortBy
    )
    {
        return sortBy switch
        {
            Sorts.IssueDateDesc => (Sorts.Descending, p => p.IssueDate),
            Sorts.IssueDateAsc => (Sorts.Ascending, p => p.IssueDate),
            Sorts.TonnageDesc => (Sorts.Descending, p => p.TonnageValue),
            Sorts.TonnageAsc => (Sorts.Ascending, p => p.TonnageValue),
            Sorts.IssuedByDesc => (Sorts.Descending, p => p.IssuedByOrg),
            Sorts.IssuedByAsc => (Sorts.Ascending, p => p.IssuedByOrg),
            Sorts.DescemberWasteDesc => (Sorts.Descending, p => p.DecemberWaste),
            Sorts.MaterialDesc => (Sorts.Descending, p => p.MaterialName),
            Sorts.MaterialAsc => (Sorts.Ascending, p => p.MaterialName),
            _ => (Sorts.Descending, p => p.IssueDate),
        };
    }

    public async Task<PaginatedResponseDto<PrnDto>> GetSearchPrnsForOrganisation(
        Guid orgId,
        PaginatedRequestDto request
    )
    {
        logger.LogInformation(
            "{Logprefix}: Repository - GetSearchPrnsForOrganisation: Organisation: {OrgId}, Request: {PaginatedRequest}",
            logPrefix,
            orgId,
            JsonConvert.SerializeObject(request)
        );
        var prns = GetAllPrnsForOrganisation(orgId);
        logger.LogInformation(
            "{Logprefix}: Repository - GetSearchPrnsForOrganisation: GetAllPrnsForOrganisation query prepared for Organisation: {OrgId}",
            logPrefix,
            orgId
        );

        var prnNumbers = prns.Select(prn => prn.PrnNumber)
            .OrderBy(prnNumber => prnNumber)
            .Distinct();
        var issuedByOrgs = prns.Select(prn => prn.IssuedByOrg)
            .OrderBy(issuedByOrg => issuedByOrg)
            .Distinct();
        var typeAhead = prnNumbers.Concat(issuedByOrgs).ToListAsync().Result;

        // Return empty response if no results
        if (!await prns.AnyAsync())
        {
            return new PaginatedResponseDto<PrnDto>
            {
                Items = [],
                TotalItems = 0,
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                SearchTerm = request.Search,
                FilterBy = request.FilterBy,
                SortBy = request.SortBy,
            };
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchPattern = $"%{request.Search}%";
            prns = prns.Where(repo =>
                EF.Functions.Like(repo.PrnNumber, searchPattern)
                || EF.Functions.Like(repo.IssuedByOrg, searchPattern)
            );
        }

        // filter by
        Expression<Func<Eprn, bool>> filterByWhereCondition = GetFilterByCondition(
            request.FilterBy
        );
        prns = prns.Where(filterByWhereCondition);

        // get the count BEFORE paging and sorting
        var totalRecords = await prns.CountAsync();

        // Sort by
        var (order, expr) = GetOrderByCondition(request.SortBy);
        prns =
            (order == Sorts.Ascending)
                ? prns.OrderBy(expr).ThenBy(p => p.PrnNumber)
                : prns.OrderByDescending(expr).ThenBy(p => p.PrnNumber);

        // Pageing
        prns = prns.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

        var prnList = await prns.Select(prn => new
            {
                prn.PrnNumber,
                prn.OrganisationName,
                PrnDto = new PrnDto
                {
                    ExternalId = prn.ExternalId,
                    PrnNumber = prn.PrnNumber,
                    OrganisationId = prn.OrganisationId,
                    MaterialName = prn.MaterialName,
                    OrganisationName = prn.OrganisationName,
                    CreatedOn = prn.CreatedOn,
                    TonnageValue = prn.TonnageValue,
                    PrnStatusId = prn.PrnStatusId,
                    PrnSignatory = prn.PrnSignatory,
                    PrnSignatoryPosition = prn.PrnSignatoryPosition,
                    IssuedByOrg = prn.IssuedByOrg,
                    IssueDate = prn.IssueDate,
                    ProcessToBeUsed = prn.ProcessToBeUsed,
                    IssuerNotes = prn.IssuerNotes,
                    DecemberWaste = prn.DecemberWaste,
                    AccreditationNumber = prn.AccreditationNumber,
                    ReprocessingSite = prn.ReprocessingSite,
                    AccreditationYear = prn.AccreditationYear,
                    ObligationYear = prn.ObligationYear,
                    LastUpdatedDate = prn.LastUpdatedDate,
                    IsExport = prn.IsExport,
                    ReprocessorExporterAgency = prn.ReprocessorExporterAgency,
                },
            })
            .ToListAsync();

        var dtoObject = new PaginatedResponseDto<PrnDto>
        {
            Items = prnList.Select(prn => prn.PrnDto).ToList(),
            TotalItems = totalRecords,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            SearchTerm = request.Search,
            FilterBy = request.FilterBy,
            SortBy = request.SortBy,
            TypeAhead = typeAhead,
        };

        logger.LogInformation(
            "{Logprefix}: Repository - GetSearchPrnsForOrganisation: returning: {DtoObject}",
            logPrefix,
            JsonConvert.SerializeObject(dtoObject)
        );
        return dtoObject;
    }

    public async Task<PaginatedResponseDto<PrnRawDataDto>> GetRawPrnData(
        string? sourceSystemId,
        PaginatedRequestDto request
    )
    {
        var page = request.Page > 0 ? request.Page : 1;
        var pageSize = request.PageSize > 0 ? request.PageSize : 10;

        logger.LogInformation(
            "{Logprefix}: Repository - GetRawPrnData: SourceSystemId: {SourceSystemId}, Request: {PaginatedRequest}",
            logPrefix,
            sourceSystemId,
            JsonConvert.SerializeObject(request)
        );

        var prns = _eprContext.Prn.AsNoTracking();

        if (string.Equals(sourceSystemId, "null", StringComparison.OrdinalIgnoreCase))
        {
            prns = prns.Where(prn => prn.SourceSystemId == null);
        }
        else if (!string.IsNullOrWhiteSpace(sourceSystemId))
        {
            prns = prns.Where(prn => prn.SourceSystemId == sourceSystemId);
        }

        var totalRecords = await prns.CountAsync();

        var items = await prns
            .OrderBy(prn => prn.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(prn => PrnRawDataDto.FromEprn(prn))
            .ToListAsync();

        var prnIds = items.Select(prn => prn.Id).ToList();
        if (prnIds.Count != 0)
        {
            var histories = await _eprContext
                .PrnStatusHistory.AsNoTracking()
                .Where(history => prnIds.Contains(history.PrnIdFk))
                .OrderBy(history => history.CreatedOn)
                .ThenBy(history => history.Id)
                .Select(history => PrnStatusHistoryRawDataDto.FromPrnStatusHistory(history))
                .ToListAsync();

            var historiesByPrnId = histories.GroupBy(history => history.PrnIdFk)
                .ToDictionary(group => group.Key, group => group.ToList());

            foreach (var item in items)
            {
                item.PrnStatusHistories =
                    historiesByPrnId.GetValueOrDefault(item.Id) ?? [];
            }
        }

        var response = new PaginatedResponseDto<PrnRawDataDto>
        {
            Items = items,
            TotalItems = totalRecords,
            CurrentPage = page,
            PageSize = pageSize,
            SearchTerm = request.Search,
            FilterBy = request.FilterBy,
            SortBy = request.SortBy,
        };

        logger.LogInformation(
            "{Logprefix}: Repository - GetRawPrnData: returning {ItemCount} Prns out of {TotalItems} matching records for page {CurrentPage} with page size {PageSize}",
            logPrefix,
            response.Items?.Count ?? 0,
            response.TotalItems,
            response.CurrentPage,
            response.PageSize
        );

        return response;
    }

    public async Task<Eprn> SavePrnDetails(Eprn entity)
    {
        try
        {
            var currentTimestamp = DateTime.UtcNow;

            var existingPrn = await _eprContext.Prn.FirstOrDefaultAsync(x =>
                x.PrnNumber == entity.PrnNumber
            );

            var statusHistory = new PrnStatusHistory
            {
                CreatedByOrganisationId = entity.OrganisationId,
                PrnStatusIdFk = entity.PrnStatusId,
                CreatedByUser = Guid.Empty,
                CreatedOn = currentTimestamp,
            };

            var prnLogVal = entity.PrnNumber?.Replace(Environment.NewLine, "");

            // Add new PRN
            if (existingPrn == null)
            {
                entity.CreatedOn = currentTimestamp;
                entity.LastUpdatedDate = currentTimestamp;
                entity.ExternalId = Guid.NewGuid();
                List<PrnStatusHistory> history = [statusHistory];
                entity.PrnStatusHistories = history;
                _eprContext.Prn.Add(entity);
                existingPrn = entity;

                logger.LogInformation(
                    "Attempting to add new Prn entity with PrnNumber : {PrnNumber}",
                    prnLogVal
                );
            }
            // Update existing PRN
            else
            {
                // the Prn has already been accepted or rejected
                if (
                    entity.PrnStatusId != existingPrn.PrnStatusId
                    && entity.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                )
                {
                    string incomingStatus = ((EprnStatus)entity.PrnStatusId).ToString();

                    // put back status and status date in Prn
                    entity.PrnStatusId = existingPrn.PrnStatusId;
                    entity.StatusUpdatedOn = existingPrn.StatusUpdatedOn;

                    // put back status in status history
                    statusHistory.PrnStatusIdFk = entity.PrnStatusId;
                    statusHistory.Comment =
                        $"{incomingStatus} => {((EprnStatus)entity.PrnStatusId).ToString()}";

                    logger.LogInformation(
                        "Resetting status history on {PrnNumber}: {Msg}",
                        prnLogVal,
                        statusHistory.Comment
                    );
                }

                entity.CreatedOn = existingPrn.CreatedOn;
                entity.LastUpdatedDate = currentTimestamp;
                entity.Id = existingPrn.Id;
                entity.ExternalId = existingPrn.ExternalId;
                _eprContext.Entry(existingPrn).State = EntityState.Modified;
                _eprContext.Entry(existingPrn).CurrentValues.SetValues(entity);

                statusHistory.PrnIdFk = existingPrn.Id;
                logger.LogInformation(
                    "Attempting to update Prn entity with PrnNumber : {PrnNumber} and {Id}",
                    prnLogVal,
                    entity?.Id
                );

                // Add Prn status history
                _eprContext.PrnStatusHistory.Add(statusHistory);
            }

            await _eprContext.SaveChangesAsync();
            logger.LogInformation(
                "Prn Entity successfully upserted. PrnNumber : {PrnNumber} and {Id}",
                prnLogVal,
                entity?.Id
            );
            return existingPrn;
        }
        catch (Exception ex)
        {
            logger.LogError(
                exception: ex,
                "{Logprefix}: Repository - SavePrnDetails: Error Message: {Message}",
                logPrefix,
                ex.Message
            );
            throw;
        }
    }

    public async Task<List<Eprn>> GetPrnsForPrnNumbers(List<string> prnNumbers)
    {
        return await _eprContext
            .Prn.Where(p => prnNumbers.Contains(p.PrnNumber))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task InsertPeprNpwdSyncPrns(List<PEprNpwdSync> syncedPrns)
    {
        await _eprContext.PEprNpwdSync.AddRangeAsync(syncedPrns);
        await _eprContext.SaveChangesAsync();
    }
}
