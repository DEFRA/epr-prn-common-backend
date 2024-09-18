namespace EPR.PRN.Backend.API.Repositories
{
    using EPR.PRN.Backend.API.Common.DTO;
    using EPR.PRN.Backend.API.Repositories.Interfaces;
    using EPR.PRN.Backend.Data;
    using EPR.PRN.Backend.Data.DataModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

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

        [ExcludeFromCodeCoverage]
        public async Task<PaginatedResponseDto<PrnDto>> GetSearchPrnsForOrganisation(Guid orgId, PaginatedRequestDto request)
        {
			var recordsPerPage = request.PageSize;
            
            var prns = _eprContext.Prn
                .Where(p => p.OrganisationId == orgId)
                .OrderByDescending(p => p.IssueDate)
                .ThenBy(p => p.PrnNumber)
                .AsQueryable();

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

            if (!string.IsNullOrWhiteSpace(request.FilterBy))
			{
				// -- TODO implement filtering
				//prns = prns.Where(e =>
				//	e.PrnHistory != null &&
				//	e.PrnHistory.Any() &&
				//	e.PrnHistory.OrderByDescending(h => h.Created).First().Status == filterByStatus);
			}

			if (!string.IsNullOrWhiteSpace(request.Search))
			{
				prns =  prns.Where(repo =>
					EF.Functions.Like(repo.PrnNumber, $"%{request.Search}%") ||
					EF.Functions.Like(repo.IssuedByOrg, $"%{request.Search}%"));
			}

			// get the count BEFORE paging and sorting
			var totalRecords = await prns.CountAsync();

			// Apply sorting after all filters
			//if (request.SortBy == "1")
			//	prns = prns.OrderBy(e => e.PrnStatusId);
			//else if (request.SortBy == "2")
			//	prns = prns.OrderBy(e => e.PrnNumber);
			//else
			//	prns = prns.OrderByDescending(repo => repo.CreatedOn);

			// Apply pagination after sorting
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
                        IssueDate = prn.IssueDate
                    }
                })
                .ToListAsync();

            return new PaginatedResponseDto<PrnDto>
            {
                Items = prnList.Select(prn => prn.PrnDto).ToList(),
                TotalItems = totalRecords,
                CurrentPage = request.Page,
                PageSize = recordsPerPage,
                SearchTerm = request.Search,
                FilterBy = request.FilterBy,
                SortBy = request.SortBy,
                TypeAhead = typeAhead
            };
        }
    }
}
