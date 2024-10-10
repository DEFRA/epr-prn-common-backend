using EPR.PRN.Backend.Obligation.Enums;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IMaterialService
    {
        Task<MaterialType?> GetMaterialByCode(string code);
    }
}
