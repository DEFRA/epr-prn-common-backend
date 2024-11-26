namespace EPR.PRN.Backend.API.Repositories
{
    using EPR.PRN.Backend.API.Common.DTO;
    using EPR.PRN.Backend.API.Repositories.Interfaces;
    using EPR.PRN.Backend.Data;
    using EPR.PRN.Backend.Data.DataModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using static EPR.PRN.Backend.API.Common.Constants.PrnConstants;

    public class Repository : IRepository
    {
        protected readonly EprContext _eprContext;
        private readonly ILogger<EprContext> _logger;

        public Repository(EprContext eprContext, ILogger<EprContext> logger)
        {
            _eprContext = eprContext;
            _logger = logger;
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

        public async Task SavePrnDetails(Eprn entity)
        {
            try
            {
                var currentUser = WindowsIdentity.GetCurrent().Name;
                var currentTimestamp = DateTime.UtcNow;

                var existingEntity = await _eprContext.Prn.FirstOrDefaultAsync(x => x.PrnNumber == entity.PrnNumber);
                
                var statusHistory = new PrnStatusHistory
                {
                    CreatedByOrganisationId = entity.OrganisationId,
                    PrnStatusIdFk = entity.PrnStatusId,
                    CreatedByUser = Guid.Empty,
                    CreatedOn = currentTimestamp,                    
                };
                
                // Add new PRN entity
                if (existingEntity == null)
                {
                    entity.CreatedBy = currentUser;
                    entity.CreatedOn = currentTimestamp;
                    _eprContext.Prn.Add(entity);     
                    statusHistory.PrnIdFk = entity.Id;

                    _logger.LogInformation("Attempting to add new Prn entity with PrnNumber : {PrnNumber}", entity?.PrnNumber);
                }
                // Update existing PRN entity
                else
                {
                    _eprContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                    statusHistory.PrnIdFk = existingEntity.Id;
                    
                    _logger.LogInformation("Attempting to update Prn entity with PrnNumber : {PrnNumber} and {Id}", entity?.PrnNumber, entity?.Id);
                }

                // Add Prn status history
                _eprContext.PrnStatusHistory.Add(statusHistory);

                await _eprContext.SaveChangesAsync();
                _logger.LogInformation("Prn Entity successfully upserted. PrnNumber : {PrnNumber} and {Id}", entity?.PrnNumber, entity?.Id);

            }
            catch (Exception ex)
            {
                _logger?.LogError(message: ex.Message, exception: ex);
                throw;
            }
        }
    }
}
