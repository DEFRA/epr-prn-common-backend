using EPR.PRN.Backend.Obligation.Enums;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IMaterialService
    {
        MaterialType? GetMaterialByCode(string code);
    }
}
