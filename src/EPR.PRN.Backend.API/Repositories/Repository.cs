namespace EPR.PRN.Backend.API.Repositories
{
    using EPR.PRN.Backend.API.Repositories.Interfaces;
    using EPR.PRN.Backend.Data;
    using Microsoft.EntityFrameworkCore;
    using DTO = EPR.PRN.Backend.API.Common.DTO;

    public class Repository : IRepository
    {
        protected readonly EprContext _eprContext;

        public Repository(
            EprContext eprContext)
        {
            _eprContext = eprContext ?? throw new ArgumentNullException(nameof(EprContext));
        }

        public async Task<DTO.PrnDTo> GetPrnById(int id)
        {
            Data.DataModels.EPRN? prn = await _eprContext
                .Prn
                .Where(a => a.Id == id)
                .Select(a => (a))
                .FirstOrDefaultAsync();
            return prn;
        }
    }
}
