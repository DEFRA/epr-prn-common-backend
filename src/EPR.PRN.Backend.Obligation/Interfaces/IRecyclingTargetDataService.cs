using EPR.PRN.Backend.Obligation.Models;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IRecyclingTargetDataService
    {
        Task<AnnualRecyclingTargets[]> GetRecyclingTargetsAsync();
    }
}
