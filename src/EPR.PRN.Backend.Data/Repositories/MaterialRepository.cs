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

        private static List<Materials> GetMaterialsFromDatabase()
        {
            // Replace with database implementation
            return new List<Materials>
            {
                new Materials { MaterialCode = "PL", MaterialName = "Plastic" },
                new Materials { MaterialCode = "WD", MaterialName = "Wood" },
                new Materials { MaterialCode = "AL", MaterialName = "Aluminium" },
                new Materials { MaterialCode = "ST", MaterialName = "Steel" },
                new Materials { MaterialCode = "PC", MaterialName = "Paper" },
                new Materials { MaterialCode = "GL", MaterialName = "Glass" }
            };
        }
    }
}
