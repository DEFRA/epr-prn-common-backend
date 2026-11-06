using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Dto;
using EPR.PRN.Backend.Data.Helpers;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class PrnRepository(EprContext context, TimeProvider? timeProvider = null) : IPrnRepository
{
    private readonly TimeProvider _timeProvider = timeProvider ?? TimeProvider.System;

    public async Task<int> GetAwaitingAcceptanceCount(Guid organisationId, int year)
    {
        return await context
            .Prn.Where(prn =>
                prn.OrganisationId == organisationId
                && prn.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
            )
            .Where(AcceptanceYearRule.IsAcceptableInto(year, _timeProvider.GetUtcNow()))
            .CountAsync();
    }

    public async Task<List<PrnObligationSummaryDto>> GetObligationSummary(
        Guid organisationId,
        int year
    )
    {
        var obligationYear = year.ToString();

        // The accepted and awaiting totals are read in two statements because they use different predicates: the
        // obligation year for accepted notes and the acceptance year rule for awaiting ones. Each is an index seek on
        // the organisation. A note accepted between the two reads can be missing from both totals in one response,
        // and the next refresh corrects it.
        var accepted = await context
            .Prn.Where(prn =>
                prn.OrganisationId == organisationId
                && prn.PrnStatusId == (int)EprnStatus.ACCEPTED
                && prn.ObligationYear == obligationYear
            )
            .GroupBy(prn => prn.MaterialName)
            .Select(group => new { MaterialName = group.Key, Tonnage = group.Sum(prn => prn.TonnageValue) })
            .ToListAsync();

        // The notes awaiting acceptance follow the same year rule as the count, so the tonnage covers exactly the counted notes.
        var awaiting = await context
            .Prn.Where(prn =>
                prn.OrganisationId == organisationId
                && prn.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
            )
            .Where(AcceptanceYearRule.IsAcceptableInto(year, _timeProvider.GetUtcNow()))
            .GroupBy(prn => prn.MaterialName)
            .Select(group => new
            {
                MaterialName = group.Key,
                Tonnage = group.Sum(prn => prn.TonnageValue),
                Count = group.Count(),
            })
            .ToListAsync();

        return accepted
            .Select(item => item.MaterialName)
            .Union(awaiting.Select(item => item.MaterialName))
            .Select(materialName => new PrnObligationSummaryDto(
                materialName,
                accepted.FirstOrDefault(item => item.MaterialName == materialName)?.Tonnage ?? 0,
                awaiting.FirstOrDefault(item => item.MaterialName == materialName)?.Tonnage ?? 0,
                awaiting.FirstOrDefault(item => item.MaterialName == materialName)?.Count ?? 0
            ))
            .ToList();
    }
}
