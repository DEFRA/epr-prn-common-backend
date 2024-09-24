namespace EPR.PRN.Backend.API.Repositories
{
    using EPR.PRN.Backend.API.Common.DTO;
    using EPR.PRN.Backend.API.Repositories.Interfaces;
    using EPR.PRN.Backend.Data;
    using EPR.PRN.Backend.Data.DataModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using System.Collections.Generic;
    using System.Linq.Expressions;

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

        private static Expression<Func<Eprn, bool>> GetFilterByCondition(string? filterBy)
        {
            return filterBy switch
            {
                "accepted-all" => p => p.PrnStatusId == (int)EprnStatus.ACCEPTED,
                "cancelled-all" => p => p.PrnStatusId == (int)EprnStatus.CANCELLED,
                "rejected-all" => p => p.PrnStatusId == (int)EprnStatus.REJECTED,
                "awaiting-all" => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE,
                "awaiting-aluminium" => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == "aluminium",
                "awaiting-glassother" => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == "glass_other",
                "awaiting-glassremelt" => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == "glass_remelt",
                "awaiting-paperfiber" => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == "paper_fiber",
                "awaiting-plastic" => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == "plastic",
                "awaiting-steel" => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == "steel",
                "awaiting-wood" => p => p.PrnStatusId == (int)EprnStatus.AWAITINGACCEPTANCE && p.MaterialName == "Wood",
                _ => sm => true
            };

        }

        private static (string order, Expression<Func<Eprn, dynamic>> expr) GetOrderByCondition(string? sortBy)
        {
            return sortBy switch
            {
                "date-issued-desc" => ("desc", p => p.IssueDate),
                "date-issued-asc" => ("asc", p => p.IssueDate),
                "tonnage-desc" => ("desc", p => p.TonnageValue),
                "tonnage-asc" => ("asc", p => p.TonnageValue),
                "issued-by-desc" => ("desc", p => p.IssuedByOrg),
                "issued-by-asc" => ("asc", p => p.IssuedByOrg),
                "december-waste-desc" => ("desc", p => p.DecemberWaste),
                "material-desc" => ("desc", p => p.MaterialName),
                "material-asc" => ("asc", p => p.MaterialName),
                _ => ("desc", p => p.IssueDate)
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

            //filter by
            Expression<Func<Eprn, bool>> filterByWhereCondition = GetFilterByCondition(request.FilterBy);
            prns = prns.Where(filterByWhereCondition);

            //Sort by
            var (order, expr) = GetOrderByCondition(request.SortBy);
            prns = (order == "asc") ? prns.OrderBy(expr).ThenBy(p => p.PrnNumber)
                : prns.OrderByDescending(expr).ThenBy(p => p.PrnNumber);

            // get the count BEFORE paging and sorting
            var totalRecords = await prns.CountAsync();

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
