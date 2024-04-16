using EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Repositories.Interfaces;
using EPR.Accreditation.API.Services.Interfaces;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Services
{
    public class PackageRecyclingNoteService : IPackageRecyclingNoteService
    {
        protected readonly IRepository _repository;

        public PackageRecyclingNoteService(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Guid> CreatePackageRecyclingNote(DTO.PackageRecyclingNote prn)
        {
            // perform checks against the prn
            if (prn == null)
                throw new ArgumentNullException(nameof(prn));

            // set the external id for the prn
            prn.ExternalId = Guid.NewGuid();
            return await _repository.AddPackageRecyclingNote(prn);
        }

        public Task<PackageRecyclingNote> GetPackageRecyclingNote(Guid externalId)
        {
            throw new NotImplementedException();
        }
    }
}
