using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Data.Interfaces
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> GetAllMaterials();
    }
}
