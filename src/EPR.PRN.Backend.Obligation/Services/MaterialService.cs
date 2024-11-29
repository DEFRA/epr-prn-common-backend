using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Interfaces;
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

        public async Task<MaterialType?> GetMaterialByCode(string code)
        {
            if (code.IsNullOrEmpty()) return null;
            var materials = await _materialRepository.GetAllMaterials();
            var material = materials.FirstOrDefault(m => m.MaterialCode == code);

            if (material != null && Enum.TryParse(material.MaterialName, true, out MaterialType materialEnum))
            {
                return materialEnum;
            }
            return null;
        }
    }
}
