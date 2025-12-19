#nullable disable

namespace EPR.PRN.Backend.API.Services;

using System.Collections.Generic;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Helpers;
using EPR.PRN.Backend.API.Models;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class PrnService(
    IRepository repository,
    ILogger<PrnService> logger,
    IConfiguration configuration
) : IPrnService
{
    private readonly string logPrefix = string.IsNullOrEmpty(configuration["LogPrefix"])
        ? "[EPR.PRN.Backend]"
        : configuration["LogPrefix"];

    public async Task<PrnDto> GetPrnForOrganisationById(Guid orgId, Guid prnId)
    {
        logger.LogInformation(
            "{Logprefix}: PrnService - GetPrnForOrganisationById: request for organisation {Organisation} and Prns to Update {PrnId}",
            logPrefix,
            orgId,
            prnId
        );
        var prns = await repository.GetPrnForOrganisationById(orgId, prnId);
        logger.LogInformation(
            "{Logprefix}: PrnService - GetPrnForOrganisationById: Prns fetched {Prns}",
            logPrefix,
            JsonConvert.SerializeObject(prns)
        );

        return prns == null ? null : (PrnDto)prns;
    }

    public async Task<List<PrnDto>> GetAllPrnByOrganisationId(Guid orgId)
    {
        logger.LogInformation(
            "{Logprefix}: PrnService - GetAllPrnByOrganisationId: request for user organisation {Organisation}",
            logPrefix,
            orgId
        );
        var prns = (await repository.GetAllPrnByOrganisationId(orgId))
            .Select(x => (PrnDto)x)
            .ToList();
        logger.LogInformation(
            "{Logprefix}: PrnService - GetAllPrnByOrganisationId: Prns fetched {Prns}",
            logPrefix,
            JsonConvert.SerializeObject(prns)
        );

        return prns;
    }

    public async Task<List<PrnUpdateStatus>> GetModifiedPrnsbyDate(
        DateTime fromDate,
        DateTime toDate
    )
    {
        var modifiedPrns = await repository.GetModifiedPrnsbyDate(fromDate, toDate);

        return modifiedPrns ?? null;
    }

    public async Task<List<PrnStatusSync>> GetSyncStatuses(DateTime fromDate, DateTime toDate)
    {
        return await repository.GetSyncStatuses(fromDate, toDate);
    }

    public async Task<PaginatedResponseDto<PrnDto>> GetSearchPrnsForOrganisation(
        Guid orgId,
        PaginatedRequestDto request
    )
    {
        logger.LogInformation(
            "{Logprefix}: PrnService - GetSearchPrnsForOrganisation: search request for user organisation {Organisation} with criteria {Request}",
            logPrefix,
            orgId,
            JsonConvert.SerializeObject(request)
        );
        var prns = await repository.GetSearchPrnsForOrganisation(orgId, request);
        logger.LogInformation(
            "{Logprefix}: PrnService - GetSearchPrnsForOrganisation: Prns fetched {Prns}",
            logPrefix,
            JsonConvert.SerializeObject(prns)
        );

        return prns;
    }

    public async Task UpdateStatus(Guid orgId, Guid userId, List<PrnUpdateStatusDto> prnUpdates)
    {
        logger.LogInformation(
            "{Logprefix}: PrnService - UpdateStatus: request for user {User}, organisation {Organisation} and Prns to Update {PrnId}",
            logPrefix,
            userId,
            orgId,
            prnUpdates
        );
        using var transaction = repository.BeginTransaction();

        logger.LogInformation(
            "{Logprefix}: PrnService - UpdateStatus: get all Prns for organisation {Organisation}",
            logPrefix,
            orgId
        );
        var prns = await repository.GetAllPrnByOrganisationId(orgId);

        if (prns.Count == 0)
        {
            logger.LogError(
                "{Logprefix}: PrnService - UpdateStatus: No Prns Found for organisation {OrgId}",
                logPrefix,
                orgId
            );
            throw new NotFoundException($"No record present for organisation {orgId}");
        }
        logger.LogInformation(
            "{Logprefix}: PrnService - UpdateStatus: Prns found for organisation {Organisation} : {Prns}",
            logPrefix,
            orgId,
            JsonConvert.SerializeObject(prns)
        );

        var nonExistingPrns = prnUpdates
            .Select(x => x.PrnId)
            .Except(prns.Select(x => x.ExternalId));
        logger.LogInformation(
            "{Logprefix}: PrnService - UpdateStatus: Filter non Existing Prns from Prns to update. {NonExistingPrns}",
            logPrefix,
            nonExistingPrns
        );

        //makes sure all the prns exsits in system
        if (nonExistingPrns.Any())
        {
            logger.LogError(
                "{Logprefix}: PrnService - UpdateStatus: No Non existing Prns Found to update",
                logPrefix
            );
            throw new NotFoundException(
                $"{string.Join(",", nonExistingPrns)} Prns doesn't exists in system"
            );
        }

        foreach (var prnUpdate in prnUpdates)
        {
            var prn = prns.Find(x => x.ExternalId == prnUpdate.PrnId);

            if (
                prn!.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                || prn!.PrnStatusId == (int)prnUpdate.Status
            )
            {
                UpdatePrn(userId, prnUpdate, prn);
            }
            else
            {
                logger.LogError(
                    "{Logprefix}: PrnService - UpdateStatus: Conflict with {Prn}, cannot be accepted or rejected please refresh and try again",
                    logPrefix,
                    prnUpdate.PrnId
                );
                throw new ConflictException(
                    $"{prnUpdate.PrnId} cannot be accepted or rejected please refresh and try again"
                );
            }
        }
        await repository.SaveTransaction(transaction);
    }

    /// <summary>
    /// This is for NPWD and will be phased out when that system is phased out
    /// </summary>
    public async Task SavePrnDetails(SavePrnDetailsRequest prn)
    {
        try
        {
            await repository.SavePrnDetails(prn.ConvertToEprn());
        }
        catch (Exception ex)
        {
            logger.LogError(
                exception: ex,
                "{Logprefix}: Error Message: {Message}",
                logPrefix,
                ex.Message
            );
            throw new OperationCanceledException(
                "Error encountered when attempting to map and save PRN requst. Please see the logs for details."
            );
        }
    }

    /// <summary>
    /// This is for RREPW
    /// </summary>
    public async Task<Eprn> SaveEprnDetails(Eprn prn)
    {
        try
        {
            return await repository.SavePrnDetails(prn);
        }
        catch (Exception ex)
        {
            logger.LogError(
                exception: ex,
                "{Logprefix}: Error Message: {Message}",
                logPrefix,
                ex.Message
            );
            throw new OperationCanceledException(
                "Error encountered when attempting to map and save PRN requst. Please see the logs for details."
            );
        }
    }

    public async Task InsertPeprNpwdSyncPrns(List<InsertSyncedPrn> syncedPrns)
    {
        List<Eprn> prns = await repository.GetPrnsForPrnNumbers(
            syncedPrns.Select(p => p.EvidenceNo).ToList()
        );
        var nonExistingPrns = syncedPrns
            .Select(x => x.EvidenceNo)
            .Except(prns.Select(x => x.PrnNumber));

        if (nonExistingPrns.Any())
        {
            logger.LogError(
                "{Logprefix}: PrnService - InsertPeprNpwdSyncPrns: No Non existing Prns Found",
                logPrefix
            );
            throw new NotFoundException(
                $"{string.Join(",", nonExistingPrns)} Prns doesn't exists in system"
            );
        }

        var currentDateTime = DateTime.UtcNow;

        var peprNpwdSyncs = syncedPrns
            .Join(
                prns,
                l => l.EvidenceNo,
                r => r.PrnNumber,
                (l, r) =>
                    new PEprNpwdSync
                    {
                        PRNId = r.Id,
                        PRNStatusId = (int)l.EvidenceStatus,
                        CreatedOn = currentDateTime,
                    }
            )
            .ToList();

        await repository.InsertPeprNpwdSyncPrns(peprNpwdSyncs);
        logger.LogInformation(
            "{Logprefix}: PrnService - InsertPeprNpwdSyncPrns: sync record inserted",
            logPrefix
        );
    }

    private void UpdatePrn(Guid userId, PrnUpdateStatusDto prnUpdate, Eprn prn)
    {
        var updateDate = DateTime.UtcNow;
        var prnStatusHistory = new PrnStatusHistory()
        {
            PrnIdFk = prn.Id,
            PrnStatusIdFk = (int)prnUpdate.Status,
            CreatedOn = updateDate,
            CreatedByUser = userId,
        };

        repository.AddPrnStatusHistory(prnStatusHistory);
        logger.LogInformation(
            "{Logprefix}: PrnService - UpdateStatus: Added Prn Status History. {PrnStatusHistory}",
            logPrefix,
            JsonConvert.SerializeObject(prnStatusHistory)
        );

        prn.PrnStatusId = (int)prnUpdate.Status;
        prn.LastUpdatedBy = userId;
        prn.LastUpdatedDate = updateDate;
        prn.StatusUpdatedOn = updateDate;
    }
}
