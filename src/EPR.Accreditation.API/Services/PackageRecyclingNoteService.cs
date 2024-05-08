using EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Repositories.Interfaces;
using EPR.Accreditation.API.Services.Interfaces;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Services
{
    public class PackageRecyclingNoteService : IPackageRecyclingNoteService
    {
        protected IRepository Repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageRecyclingNoteService"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public PackageRecyclingNoteService(IRepository repository)
        {
            ArgumentNullException.ThrowIfNull(repository);
            this.Repository = repository;
        }

        /// <inheritdoc/>
        public async Task<Guid> CreatePackageRecyclingNote(DTO.PackageRecyclingNoteRequest prn)
            => await this.Repository.AddPackageRecyclingNote(prn);

        /// <inheritdoc/>
        public async Task UpdatePrn(Guid prnId, DTO.PackageRecyclingNoteRequest newData)
        {
            await this.Repository.UpdatePrn(prnId, newData);
        }

        /// <inheritdoc/>
        public async Task<PackageRecyclingNoteResponse> GetPackageRecyclingNote(Guid externalId)
            => await this.Repository.GetPackageRecyclingNote(externalId);

        /// <inheritdoc/>
        public async Task<IEnumerable<Guid>> GetPrnsForOrganisation(Guid organisationId)
            => await this.Repository.GetPrnsForOrganisation(organisationId);

        /// <inheritdoc/>
        public async Task UpdatePrnStatus(Guid prnId, DTO.PrnStatusHistoryRequest status)
            => await this.Repository.UpdatePrnStatus(prnId, status);

        /// <inheritdoc/>
        public async Task DeletePrn(Guid prnId)
            => await this.Repository.DeletePrn(prnId);
    }
}
