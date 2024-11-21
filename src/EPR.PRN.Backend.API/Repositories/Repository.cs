namespace EPR.PRN.Backend.API.Repositories
{
    using EPR.PRN.Backend.API.Common.DTO;
    using EPR.PRN.Backend.API.Common.Enums;
    using EPR.PRN.Backend.API.Repositories.Interfaces;
    using EPR.PRN.Backend.Data;
    using EPR.PRN.Backend.Data.DataModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using static EPR.PRN.Backend.API.Common.Constants.PrnConstants;

    public class Repository : IRepository
    {
        protected readonly EprContext _eprContext;

        public Repository(EprContext eprContext)
        {
            _eprContext = eprContext;
        }

        private IQueryable<Eprn> GetAllPrnsForOrganisation(Guid orgId)
        {
            return _eprContext.Prn.Where(x => x.OrganisationId == orgId);
        }
        public async Task<List<Eprn>> GetAllPrnByOrganisationId(Guid orgId)
        {
            return await GetAllPrnsForOrganisation(orgId)
                        .ToListAsync();
        }

        public async Task<List<PrnUpdateStatus>> GetModifiedPrnsbyDate(DateTime fromDate, DateTime toDate)
        {
            var result = await (from p in _eprContext.Prn
                                join ps in _eprContext.PrnStatus on p.PrnStatusId equals ps.Id
                                where p.LastUpdatedDate >= fromDate && p.LastUpdatedDate <= toDate
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
                EvidenceStatusCode = GetEvidenceStatusCodeString(
                    MapStatusCode((EprnStatus)Enum.Parse(typeof(EprnStatus), p.StatusName), p.AccreditationYear)),
                StatusDate = p.StatusUpdatedOn,
                AccreditationYear = p.AccreditationYear
            }).ToList();

            return prnUpdateStatuses;
        }

        public async Task<Eprn?> GetPrnForOrganisationById(Guid orgId, Guid prnId)
        {
            return await GetAllPrnsForOrganisation(orgId)
                        .FirstOrDefaultAsync(p => p.ExternalId == prnId);
        }

        public async Task SaveTransaction(IDbContextTransaction transaction)
        {
            await _eprContext.SaveChangesAsync();
            transaction.Commit();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _eprContext.Database.BeginTransaction();
        }

        public void AddPrnStatusHistory(PrnStatusHistory prnStatusHistory)
        {
            _eprContext.PrnStatusHistory.Add(prnStatusHistory);
        }

        private static string GetEvidenceStatusCodeString(EvidenceStatusCode statusCode)
        {
            return Enum.GetName(typeof(EvidenceStatusCode), statusCode);
        }

        private EvidenceStatusCode MapStatusCode(EprnStatus status, string accreditationYear)
        {
            switch (status)
            {
                case EprnStatus.ACCEPTED:
                    return EvidenceStatusCode.EV_ACCEP;
                case EprnStatus.REJECTED:
                    return EvidenceStatusCode.EV_ACANCEL;
                case EprnStatus.CANCELLED:
                    return EvidenceStatusCode.EV_CANCEL;
                case EprnStatus.AWAITINGACCEPTANCE:
                    if (accreditationYear == "2024")
                        return EvidenceStatusCode.EV_AWACCEP;
                    else if (accreditationYear == "2025")
                        return EvidenceStatusCode.EV_AWACCEP_EPR;
                    else
                        return EvidenceStatusCode.EV_AWACCEP; // Default case, assuming for any other years it maps to EV_AWACCEP
                default:
                    throw new ArgumentException($"Unknown status: {status}");
            }
        }


        private static Expression<Func<Eprn, bool>> GetFilterByCondition(string? filterBy)
        {
            return filterBy switch
            {
                Filters.AcceptedAll =>
                    p => p.PrnStatusId == (int)EprnStatus.ACCEPTED,

                Filters.CancelledAll =>
                    p => p.PrnStatusId == (int)EprnStatus.CANCELLED,

                Filters.RejectedAll =>
                    p => p.PrnStatusId == (int)EprnStatus.REJECTED,

                Filters.AwaitingAll =>
                    p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE,

                Filters.AwaitingAluminium =>
                    p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                    && p.MaterialName == Common.Constants.PrnConstants.Materials.Aluminium,

                Filters.AwaitingGlassOther =>
                    p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                    && p.MaterialName == Common.Constants.PrnConstants.Materials.GlassOther,

                Filters.AwaitingGlassMelt =>
                    p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                    && p.MaterialName == Common.Constants.PrnConstants.Materials.GlassMelt,

                Filters.AwaitngPaperFiber =>
                    p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                    && p.MaterialName == Common.Constants.PrnConstants.Materials.PaperFiber,

                Filters.AwaitngPlastic =>
                    p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                    && p.MaterialName == Common.Constants.PrnConstants.Materials.Plastic,

                Filters.AwaitngSteel =>
                    p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                    && p.MaterialName == Common.Constants.PrnConstants.Materials.Steel,

                Filters.AwaitngWood =>
                    p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE
                    && p.MaterialName == Common.Constants.PrnConstants.Materials.Wood,

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

            var prns = GetAllPrnsForOrganisation(orgId);

            var prnNumbers = prns
                .Select(prn => prn.PrnNumber)
                .OrderBy(prnNumber => prnNumber)
                .Distinct();
            var issuedByOrgs = prns
                .Select(prn => prn.IssuedByOrg)
                .OrderBy(issuedByOrg => issuedByOrg)
                .Distinct();
            var typeAhead = prnNumbers
                .Concat(issuedByOrgs)
                .ToList();

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
                prns = prns.Where(repo =>
                    EF.Functions.Like(repo.PrnNumber, searchPattern) ||
                    EF.Functions.Like(repo.IssuedByOrg, searchPattern));
            }

            // filter by
            Expression<Func<Eprn, bool>> filterByWhereCondition = GetFilterByCondition(request.FilterBy);
            prns = prns.Where(filterByWhereCondition);

            // get the count BEFORE paging and sorting
            var totalRecords = await prns.CountAsync();

            // Sort by
            var (order, expr) = GetOrderByCondition(request.SortBy);
            prns = (order == Sorts.Ascending) ? prns.OrderBy(expr).ThenBy(p => p.PrnNumber)
                : prns.OrderByDescending(expr).ThenBy(p => p.PrnNumber);

            // Pageing
            prns = prns
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

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
                })
                .ToListAsync();

            return new PaginatedResponseDto<PrnDto>
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
        }
    }
}
