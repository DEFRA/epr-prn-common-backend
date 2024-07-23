
namespace EPR.PRN.Backend.API.Repositories.Interfaces
{
    using DTO = EPR.PRN.Backend.API.Common.DTO;

    public interface IRepository
    {
        Task<DTO.PrnDTo> GetPrnById(int id);

        Task<DTO.PrnDTo> GetPrnById(Guid id);

        Task<List<DTO.PrnDTo>> GetAllPrnByOrganisationId(Guid id);

        Task UpdatePrn(Guid id, DTO.PrnDTo prn);
    }
}
