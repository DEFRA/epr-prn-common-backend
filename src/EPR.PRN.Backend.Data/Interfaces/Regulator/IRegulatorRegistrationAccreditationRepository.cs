using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator;

public interface IRegulatorRegistrationAccreditationRepository
{
    Task<Accreditation> GetAccreditationById(Guid accreditationId);
    Task<Accreditation> GetAccreditationPaymentFeesById(Guid accreditationId);
    Task AccreditationMarkAsDulyMade(Guid accreditationId, int statusId, DateTime DulyMadeDate, DateTime DeterminationDate, Guid DulyMadeBy);

}
