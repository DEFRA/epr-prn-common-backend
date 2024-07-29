namespace EPR.PRN.Backend.API.Repositories
{
    using EPR.PRN.Backend.API.Repositories.Interfaces;
    using EPR.PRN.Backend.Data;
    using EPR.PRN.Backend.Data.DataModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using System.Collections.Generic;

    public class Repository : IRepository
    {
        protected readonly EprContext _eprContext;

        public Repository(
            EprContext eprContext)
        {
            _eprContext = eprContext ?? throw new ArgumentNullException(nameof(EprContext));
        }
        private IQueryable<EPRN> GetAllPrnsForOrganisation(Guid orgId)
        {
            return _eprContext.Prn.Where(x => x.OrganisationId == orgId);
        }
        public async Task<List<EPRN>> GetAllPrnByOrganisationId(Guid orgId)
        {
            return await GetAllPrnsForOrganisation(orgId)
                        .ToListAsync();
        }

        public async Task<EPRN?> GetPrnForOrganisationById(Guid orgId, Guid prnId)
        {
            return await GetAllPrnsForOrganisation(orgId)
                        .FirstOrDefaultAsync(p => p.ExternalId == prnId);
        }

        //public async Task UpdateStatus(Guid orgId, List<PrnUpdateStatusDto> prnUpdates)
        //{
        //    using var transaction = BeginTransaction();
        //    var prns = await GetAllPrnByOrganisationId(orgId);

        //    foreach (var prnUpdate in prnUpdates)
        //    {
        //        var prn = prns.Find(x => x.ExternalId == prnUpdate.PrnId);
        //        prn!.PrnStatusId = (int)prnUpdate.Status;
        //    }
        //    await SaveTransaction(transaction);
        //}

        public async Task SaveTransaction(IDbContextTransaction transaction)
        {
            await _eprContext.SaveChangesAsync();
            transaction.Commit();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _eprContext.Database.BeginTransaction();
        }
    }
}
