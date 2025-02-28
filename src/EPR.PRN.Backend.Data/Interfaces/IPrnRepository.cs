﻿using EPR.PRN.Backend.Data.Dto;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IPrnRepository
    {
        IQueryable<EprnResultsDto> GetAcceptedAndAwaitingPrnsByYear(IEnumerable<Guid> organisationIds, int year);
    }
}
