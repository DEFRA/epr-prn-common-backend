namespace EPR.PRN.Backend.Data.Interfaces.Registrationreprocessingio;

public interface IRegistrationReprocessorIORepository
{
    Task CreateReprocessorOutputAsync(
        Guid ReprocessorOutputId,
        int RegistrationMaterialId,
        decimal SentToOtherSiteTonnes,
        decimal ContaminantTonnes,
        decimal ProcessLossTonnes,
        decimal TotalOutputTonnes,
        List<KeyValuePair<string, decimal>> MaterialorProducts
    );
}
