using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class MaterialService : IMaterialService
    {
        public MaterialType? GetMaterialByCode(string code)
        {
            if (code.IsNullOrEmpty()) return null;
            var materials = GetMaterialsFromDatabase();
            if (materials.TryGetValue(code, out var material))
            {
                if (Enum.TryParse(material, true, out MaterialType materialEnum))
                {
                    return materialEnum;
                }
            }
            return null;
        }

        private Dictionary<string, string> GetMaterialsFromDatabase()
        {
            //Replace with database implementation
            return new Dictionary<string, string>
            {
                { "PL", "Plastic" },
                { "WD", "Wood" },
                { "AL", "Aluminium" },
                { "ST", "Steel" },
                { "PC", "Paper" },
                { "GL", "Glass" }
            };
        }
    }
}
