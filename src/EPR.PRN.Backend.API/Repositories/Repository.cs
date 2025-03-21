﻿namespace EPR.PRN.Backend.API.Repositories;

using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq.Expressions;
using static EPR.PRN.Backend.API.Common.Constants.PrnConstants;

public class Repository(EprContext eprContext, ILogger<Repository> logger, IConfiguration configuration) : IRepository
{
    protected readonly EprContext _eprContext = eprContext;
    private readonly string logPrefix = configuration["LogPrefix"];

    private IQueryable<Eprn> GetAllPrnsForOrganisation(Guid orgId)
    {
        logger.LogInformation("{Logprefix}: Repository - GetAllPrnsForOrganisation: request for organisation {Organisation}", logPrefix, orgId);
        var prns = _eprContext.Prn.Where(x => x.OrganisationId == orgId);
        logger.LogInformation("{Logprefix}: PrnService - GetAllPrnsForOrganisation: Prns fetched {Prns}", logPrefix, JsonConvert.SerializeObject(prns));
        return prns;
    }

    public async Task<List<Eprn>> GetAllPrnByOrganisationId(Guid orgId)
    {
        logger.LogInformation("{Logprefix}: Repository - GetAllPrnByOrganisationId: request for organisation {Organisation}", logPrefix, orgId);
        var prns = await GetAllPrnsForOrganisation(orgId).ToListAsync();
        logger.LogInformation("{Logprefix}: PrnService - GetAllPrnByOrganisationId: Prns fetched {Prns}", logPrefix, JsonConvert.SerializeObject(prns));
        return prns;
    }

    public async Task<List<PrnUpdateStatus>> GetModifiedPrnsbyDate(DateTime fromDate, DateTime toDate)
    {
        var result = await (from p in _eprContext.Prn
                            join ps in _eprContext.PrnStatus on p.PrnStatusId equals ps.Id
                            where p.StatusUpdatedOn >= fromDate && p.StatusUpdatedOn <= toDate
                            && (p.PrnStatusId == 1 || p.PrnStatusId == 2)
                            && !_eprContext.PEprNpwdSync.Any(s => p.Id == s.PRNId && p.PrnStatusId == s.PRNStatusId)
                            select new
                            {
                                p.PrnNumber,
                                ps.StatusName,
                                p.StatusUpdatedOn,
                                p.AccreditationYear
                            }).ToListAsync();

        var prnUpdateStatuses = result.Select(p => new PrnUpdateStatus
        {
            EvidenceNo = p.PrnNumber,
            EvidenceStatusCode = MapStatusCode((EprnStatus)Enum.Parse(typeof(EprnStatus), p.StatusName)),
            StatusDate = p.StatusUpdatedOn.Value.ToUniversalTime(),
            AccreditationYear = p.AccreditationYear
        }).ToList();

        return prnUpdateStatuses;
    }

    public async Task<List<PrnStatusSync>> GetSyncStatuses(DateTime fromDate, DateTime toDate)
    {
        var result = await (from p in _eprContext.Prn
                            join ps in _eprContext.PEprNpwdSync on p.Id equals ps.PRNId
                            where ps.CreatedOn >= fromDate && ps.CreatedOn < toDate
                            select new
                            {
                                p.PrnNumber,
                                ps.PRNStatusId,
                                p.OrganisationName,
                                ps.CreatedOn
                            }).ToListAsync();

        var prnStatusSync = result.Select(p => new PrnStatusSync
        {
            PrnNumber = p.PrnNumber,
            StatusName = MapStatusCode((EprnStatus)p.PRNStatusId),
            OrganisationName = p.OrganisationName,
            UpdatedOn = p.CreatedOn
        }).ToList();

        return prnStatusSync;
    }

    public async Task<Eprn?> GetPrnForOrganisationById(Guid orgId, Guid prnId)
    {
        logger.LogInformation("{Logprefix}: Repository - GetPrnForOrganisationById: request for organisation {Organisation}, Prn {PrnId}", logPrefix, orgId, prnId);
        var eprn = await GetAllPrnsForOrganisation(orgId).FirstOrDefaultAsync(p => p.ExternalId == prnId);
        logger.LogInformation("{Logprefix}: Repository - GetPrnForOrganisationById: Eprn fetched {Eprn}", logPrefix, JsonConvert.SerializeObject(eprn));
        return eprn;
    }

    public async Task SaveTransaction(IDbContextTransaction transaction)
    {
        logger.LogInformation("{Logprefix}: Repository - SaveTransaction: Saving Transaction", logPrefix);
        await _eprContext.SaveChangesAsync();
        transaction.Commit();
    }

    public IDbContextTransaction BeginTransaction()
    {
        return _eprContext.Database.BeginTransaction();
    }

    public void AddPrnStatusHistory(PrnStatusHistory prnStatusHistory)
    {
        logger.LogInformation("{Logprefix}: Repository - AddPrnStatusHistory: {PrnStatusHistory}", logPrefix, JsonConvert.SerializeObject(prnStatusHistory));
        _eprContext.PrnStatusHistory.Add(prnStatusHistory);
    }

    private string MapStatusCode(EprnStatus status)
    {
        switch (status)
        {
            case EprnStatus.ACCEPTED:
                return EvidenceStatusCode.EV_ACCEP.ToHyphenatedString();
            case EprnStatus.REJECTED:
                return EvidenceStatusCode.EV_ACANCEL.ToHyphenatedString();
            default:
                throw new ArgumentException($"Unknown status: {status}");
        }
    }   

    private static Expression<Func<Eprn, bool>> GetFilterByCondition(string? filterBy)
    {
        return filterBy switch
        {
            Filters.AcceptedAll => p => p.PrnStatusId == (int)EprnStatus.ACCEPTED,

            Filters.CancelledAll => p => p.PrnStatusId == (int)EprnStatus.CANCELLED,

            Filters.RejectedAll => p => p.PrnStatusId == (int)EprnStatus.REJECTED,

            Filters.AwaitingAll => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE,

            Filters.AwaitingAluminium => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == Common.Constants.PrnConstants.Materials.Aluminium,

            Filters.AwaitingGlassOther => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == Common.Constants.PrnConstants.Materials.GlassOther,

            Filters.AwaitingGlassMelt => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == Common.Constants.PrnConstants.Materials.GlassMelt,

            Filters.AwaitngPaperFiber => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == Common.Constants.PrnConstants.Materials.PaperFiber,

            Filters.AwaitngPlastic => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == Common.Constants.PrnConstants.Materials.Plastic,

            Filters.AwaitngSteel => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == Common.Constants.PrnConstants.Materials.Steel,

            Filters.AwaitngWood => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == Common.Constants.PrnConstants.Materials.Wood,

            _ => sm => true
        };

    }

    private static (string order, Expression<Func<Eprn, dynamic>> expr) GetOrderByCondition(string? sortBy)
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
            _ => (Sorts.Descending, p => p.IssueDate)
        };
    }

    public async Task<PaginatedResponseDto<PrnDto>> GetSearchPrnsForOrganisation(Guid orgId, PaginatedRequestDto request)
    {
        logger.LogInformation("{Logprefix}: Repository - GetSearchPrnsForOrganisation: Organisation: {OrgId}, Request: {PaginatedRequest}", logPrefix, orgId, JsonConvert.SerializeObject(request));
        var prns = GetAllPrnsForOrganisation(orgId);
        logger.LogInformation("{Logprefix}: Repository - GetSearchPrnsForOrganisation: GetAllPrnsForOrganisation for Organisation: {OrgId}, Fetched: {PaginatedRequest}", logPrefix, orgId, JsonConvert.SerializeObject(prns));

        var prnNumbers = prns.Select(prn => prn.PrnNumber).OrderBy(prnNumber => prnNumber).Distinct();
        var issuedByOrgs = prns.Select(prn => prn.IssuedByOrg).OrderBy(issuedByOrg => issuedByOrg).Distinct();
        var typeAhead = prnNumbers.Concat(issuedByOrgs).ToListAsync().Result;

        // Return empty response if no results
        if (!prns.Any())
        {
            return new PaginatedResponseDto<PrnDto>
            {
                Items = [],
                TotalItems = 0,
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                SearchTerm = request.Search,
                FilterBy = request.FilterBy,
                SortBy = request.SortBy
            };
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchPattern = $"%{request.Search}%";
            prns = prns.Where(repo => EF.Functions.Like(repo.PrnNumber, searchPattern) || EF.Functions.Like(repo.IssuedByOrg, searchPattern));
        }

        // filter by
        Expression<Func<Eprn, bool>> filterByWhereCondition = GetFilterByCondition(request.FilterBy);
        prns = prns.Where(filterByWhereCondition);

        // get the count BEFORE paging and sorting
        var totalRecords = await prns.CountAsync();

        // Sort by
        var (order, expr) = GetOrderByCondition(request.SortBy);
        prns = (order == Sorts.Ascending) ? prns.OrderBy(expr).ThenBy(p => p.PrnNumber) : prns.OrderByDescending(expr).ThenBy(p => p.PrnNumber);

        // Pageing
        prns = prns.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

        var prnList = await prns
            .Select(prn => new
            {
                prn.PrnNumber,
                prn.OrganisationName,
                PrnDto = new PrnDto
                {
                    ExternalId = prn.ExternalId,
                    PrnNumber = prn.PrnNumber,
                    MaterialName = prn.MaterialName,
                    OrganisationName = prn.OrganisationName,
                    CreatedOn = prn.CreatedOn,
                    TonnageValue = prn.TonnageValue,
                    PrnStatusId = prn.PrnStatusId,
                    IssuedByOrg = prn.IssuedByOrg,
                    IssueDate = prn.IssueDate,
                    IssuerNotes = prn.IssuerNotes,
                    DecemberWaste = prn.DecemberWaste
                }
            }).ToListAsync();

        var dtoObject = new PaginatedResponseDto<PrnDto>
        {
            Items = prnList.Select(prn => prn.PrnDto).ToList(),
            TotalItems = totalRecords,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            SearchTerm = request.Search,
            FilterBy = request.FilterBy,
            SortBy = request.SortBy,
            TypeAhead = typeAhead
        };

        logger.LogInformation("{Logprefix}: Repository - GetSearchPrnsForOrganisation: returning: {DtoObject}", logPrefix, JsonConvert.SerializeObject(dtoObject));
        return dtoObject;
    }

    public async Task SavePrnDetails(Eprn newPrn)
    {
        try
        {
            var currentTimestamp = DateTime.UtcNow;

            var existingPrn = await _eprContext.Prn.FirstOrDefaultAsync(x => x.PrnNumber == newPrn.PrnNumber);

            var statusHistory = new PrnStatusHistory
            {
                CreatedByOrganisationId = newPrn.OrganisationId,
                PrnStatusIdFk = newPrn.PrnStatusId,
                CreatedByUser = Guid.Empty,
                CreatedOn = currentTimestamp,
            };

            var prnLogVal = newPrn.PrnNumber?.Replace(Environment.NewLine, "");

            // Add new PRN
            if (existingPrn == null)
            {
                newPrn.CreatedOn = currentTimestamp;
                newPrn.LastUpdatedDate = currentTimestamp;
                newPrn.ExternalId = Guid.NewGuid();
                List<PrnStatusHistory> history = [statusHistory];
                newPrn.PrnStatusHistories = history;
                _eprContext.Prn.Add(newPrn);

                logger.LogInformation("Attempting to add new Prn entity with PrnNumber : {PrnNumber}", prnLogVal);
            }
            // Update existing PRN
            else
            {
                // the Prn has already been accepted or rejected
                if (newPrn.PrnStatusId != existingPrn.PrnStatusId && newPrn.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE)
                {
                    string incomingStatus = ((EprnStatus)newPrn.PrnStatusId).ToString();

                    // put back status and status date in Prn
                    newPrn.PrnStatusId = existingPrn.PrnStatusId;
                    newPrn.StatusUpdatedOn = existingPrn.StatusUpdatedOn;
                    
                    // put back status in status history
                    statusHistory.PrnStatusIdFk = newPrn.PrnStatusId;
                    statusHistory.Comment = $"{incomingStatus} => {((EprnStatus)newPrn.PrnStatusId).ToString()}";

                    logger.LogInformation("Resetting status history on {PrnNumber}: {Msg}", prnLogVal, statusHistory.Comment);
                }

                newPrn.CreatedOn = existingPrn.CreatedOn;
                newPrn.LastUpdatedDate = currentTimestamp;
                newPrn.Id = existingPrn.Id;
                newPrn.ExternalId = existingPrn.ExternalId;
                _eprContext.Entry(existingPrn).State = EntityState.Modified;
                _eprContext.Entry(existingPrn).CurrentValues.SetValues(newPrn);

                statusHistory.PrnIdFk = existingPrn.Id;
                logger.LogInformation("Attempting to update Prn entity with PrnNumber : {PrnNumber} and {Id}", prnLogVal, newPrn?.Id);

                // Add Prn status history
                _eprContext.PrnStatusHistory.Add(statusHistory);
            }

            await _eprContext.SaveChangesAsync();
            logger.LogInformation("Prn Entity successfully upserted. PrnNumber : {PrnNumber} and {Id}", prnLogVal, newPrn?.Id);

        }
        catch (Exception ex)
        {
            logger?.LogError(message: ex.Message, exception: ex);
            throw;
        }
    }

    public async Task<List<Eprn>> GetPrnsForPrnNumbers(List<string> prnNumbers)
    {
        return await _eprContext.Prn.Where(p => prnNumbers.Contains(p.PrnNumber)).AsNoTracking().ToListAsync();
    }

    public async Task InsertPeprNpwdSyncPrns(List<PEprNpwdSync> syncedPrns)
    {
        await _eprContext.PEprNpwdSync.AddRangeAsync(syncedPrns);
        await _eprContext.SaveChangesAsync();
    }
}
