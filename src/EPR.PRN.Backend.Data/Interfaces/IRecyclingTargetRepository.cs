using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IRecyclingTargetRepository
    {
        Task<IEnumerable<RecyclingTarget>> GetAllAsync();
    }
}