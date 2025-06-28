using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class MaterialRepository(EprContext context) : IMaterialRepository
    {
        public async Task<IEnumerable<Material>> GetAllMaterials()
        {
            return await context.Material
                                .AsNoTracking()
                                .Include(m => m.PrnMaterialMappings)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Material>> GetMaterialsByRegistrationIdQuery(Guid registrationId)
        {
            var materialsInRegistrationMaterial = await context.RegistrationMaterials.Where(m =>m.Registration.ExternalId == registrationId)
                                                                                     .Select(m => m.MaterialId)
                                                                                     .ToListAsync();
            
            var materialsNotInRegistrationMaterial = await context.Material.Where(m => !materialsInRegistrationMaterial.Contains(m.Id)).ToListAsync();

            return materialsNotInRegistrationMaterial;
        }
    }
}