using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Dto;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class PrnRepository(EprContext context) : IPrnRepository
{
    public async Task<List<PrnObligationSummaryDto>> GetObligationSummary(
        Guid organisationId,
        int year
    )
    {
        var currentYear = year.ToString();
        var previousYear = (year - 1).ToString();

        return await context
            .Prn.Where(prn =>
                prn.OrganisationId == organisationId
                && (
                    (
                        prn.PrnStatusId == (int)EprnStatus.ACCEPTED
                        && prn.ObligationYear == currentYear
                    )
                    || (
                        prn.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                        && (
                            prn.ObligationYear == currentYear
                            || (prn.AccreditationYear == previousYear && prn.DecemberWaste)
                        )
                    )
                )
            )
            .GroupBy(prn => prn.MaterialName)
            .Select(group => new PrnObligationSummaryDto(
                group.Key,
                group
                    .Where(prn => prn.PrnStatusId == (int)EprnStatus.ACCEPTED)
                    .Sum(prn => prn.TonnageValue),
                group
                    .Where(prn => prn.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE)
                    .Sum(prn => prn.TonnageValue),
                group.Count(prn => prn.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE)
            ))
            .ToListAsync();
    }
}
