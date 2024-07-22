
namespace EPR.PRN.Backend.API.Repositories.Interfaces
{
    using DTO = EPR.PRN.Backend.API.Common.DTO;

    public interface IRepository
    {
        Task<DTO.PrnDTo> GetPrnById(int id);

        Task<List<DTO.PrnDTo>> GetAllPrnByOrganisationId(Guid id);
    }
}
