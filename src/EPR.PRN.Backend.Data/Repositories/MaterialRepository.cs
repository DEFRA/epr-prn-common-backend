using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;

namespace EPR.PRN.Backend.Data.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        public IEnumerable<Materials> GetAllMaterials()
        {
            return GetMaterialsFromDatabase();
        }

        private IEnumerable<Materials> GetMaterialsFromDatabase()
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
