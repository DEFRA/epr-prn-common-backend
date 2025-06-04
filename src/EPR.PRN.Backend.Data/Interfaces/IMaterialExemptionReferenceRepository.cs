using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces;

public interface IMaterialExemptionReferenceRepository
{
    Task<bool> CreateMaterialExemptionReference(List<MaterialExemptionReference> exemptions);
}
