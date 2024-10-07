using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class MaterialRepository(EprContext context) : IMaterialRepository
    {
        public async Task<IEnumerable<Materials>> GetAllMaterials()
        {
            return await context.Materials.ToListAsync();
        }
    }
}
