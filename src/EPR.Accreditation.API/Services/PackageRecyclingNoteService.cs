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

        public async Task CreatePackageRecyclingNote(DTO.PackageRecyclingNote prn)
        {
            // perform checks against the prn
            if (prn == null)
                throw new ArgumentNullException(nameof(prn));

            // set the external id for the prn
            prn.ExternalId = Guid.NewGuid();
            await _repository.AddPackageRecyclingNote(prn);
        }

        public async Task<PackageRecyclingNote> GetPackageRecyclingNote(Guid externalId)
            => await _repository.GetPackageRecyclingNote(externalId);

        public async Task<IEnumerable<Guid>> GetPrnsForOrganisation(Guid organisationId)
            => await _repository.GetPrnsForOrganisation(organisationId);

        public async Task DeletePrn(Guid prnId)
            => await _repository.DeletePrn(prnId);
    }
}
