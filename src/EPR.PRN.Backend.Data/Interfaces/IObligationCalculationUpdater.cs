namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IObligationCalculationUpdater
    {
        Task<int> SoftDeleteBySubmitterAndYearAsync(Guid submitterId, int year);
    }
}