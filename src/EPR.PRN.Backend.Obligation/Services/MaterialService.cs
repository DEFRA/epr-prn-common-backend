using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public MaterialType? GetMaterialByCode(string code)
        {
            if (code.IsNullOrEmpty()) return null;
            var materials = _materialRepository.GetAllMaterials().ToList();
            var material = materials.Find(m => m.MaterialCode == code);

            if (material != null && Enum.TryParse(material.MaterialName, true, out MaterialType materialEnum))
            {
                return materialEnum;
            }
            return null;
        }
    }
}
