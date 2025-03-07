using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IMaterialService
    {
        Task<MaterialType?> GetMaterialByCode(string code);
	}
}
