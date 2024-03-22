using EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Services.Interfaces
{
    public interface IMaterialService
    {
        Task<IEnumerable<Material>> GetMaterialList();
    }
}
