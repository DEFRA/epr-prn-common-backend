using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Accreditations;

public interface IAccreditationPrnIssueAuthRepository
{
    Task<List<AccreditationPrnIssueAuth>?> GetByAccreditationId(Guid accreditationId);

    Task ReplaceAllByAccreditationId(Guid accreditationId, List<AccreditationPrnIssueAuth> accreditationPrnIssueAuths);
}
