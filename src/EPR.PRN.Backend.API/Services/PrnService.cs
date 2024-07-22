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
            var response = await _repository.GetPrnById(Id);
            response.AccreditationNumber = "99999";
            return response;
        }
    }
}
