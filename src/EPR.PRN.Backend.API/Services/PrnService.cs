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

        public async Task<DTO.PrnDTo> GetPrnById(int id)
        {
            return await _repository.GetPrnById(id);
        }

        public async Task<List<DTO.PrnDTo>> GetAllPrnByOrganisationId(Guid organisationId)
        {
            return await _repository.GetAllPrnByOrganisationId(organisationId);
        }

        public async Task AcceptPrn(int id)
        {
            var existingPrn = this.GetPrnById(id);
            if (existingPrn != null) 
            {
                existingPrn.Result.PrnStatusId = 2;
                await _repository.UpdatePrn(id, existingPrn.Result);
            }
        }
    }
}
