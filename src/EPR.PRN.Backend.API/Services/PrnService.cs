namespace EPR.PRN.Backend.API.Services
{
    using EPR.PRN.Backend.API.Repositories.Interfaces;
    using EPR.PRN.Backend.API.Services.Interfaces;
    using DTO = EPR.PRN.Backend.API.Common.DTO;

    public class PrnService : IPrnService
    {
        protected readonly IRepository _repository;

        public PrnService(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<DTO.PrnDTo> GetPrnById(int Id)
        {
            return await _repository.GetPrnById(Id);
        }

        public async Task<List<DTO.PrnDTo>> GetAllPrnByOrganisationId(Guid organisationId)
        {
            return await _repository.GetAllPrnByOrganisationId(organisationId);
        }
    }
}
