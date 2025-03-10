﻿using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Dto;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories;

public class PrnRepository(EprContext context) : IPrnRepository
{
    public IQueryable<EprnResultsDto> GetAcceptedAndAwaitingPrnsByYear(Guid organisationId, int year)
    {
        return context.Prn.Join(
                context.PrnStatus, 
                eprn => eprn.PrnStatusId, 
                status => status.Id, (eprn, status) => new EprnResultsDto { Eprn = eprn, Status = status }
                ).Where(joined => joined.Eprn.OrganisationId == organisationId && (joined.Status.StatusName == EprnStatus.ACCEPTED.ToString()
                || joined.Status.StatusName == EprnStatus.AWAITINGACCEPTANCE.ToString()) && joined.Eprn.ObligationYear == year.ToString()).AsNoTracking();
    }
}
