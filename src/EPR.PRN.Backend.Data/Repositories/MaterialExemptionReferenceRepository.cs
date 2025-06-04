using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;

namespace EPR.PRN.Backend.Data.Repositories;

public class MaterialExemptionReferenceRepository(EprContext context) : IMaterialExemptionReferenceRepository
{
    public async Task<bool> CreateMaterialExemptionReference(List<MaterialExemptionReference> exemptions)
    {
        try
        {
            await context.MaterialExemptionReferences.AddRangeAsync(exemptions);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }
        catch
        {
            return false;
        }
    }
}
