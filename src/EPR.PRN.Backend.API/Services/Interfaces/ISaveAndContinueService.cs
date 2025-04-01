namespace EPR.PRN.Backend.API.Services.Interfaces
{
    public interface ISaveAndContinueService
    {
        Task AddAsync(int registrationId, string area, string action, string controller, string parameters);
    }
}
