using EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Repositories.Interfaces;
using EPR.Accreditation.API.Services.Interfaces;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Services
{
    public class AccreditationService : IAccreditationService
    {
        protected readonly IRepository _repository;

        public AccreditationService(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Guid> CreateAccreditation(DTO.Accreditation accreditation)
        {
            // perform checks against the accreditation
            if (accreditation == null)
                throw new ArgumentNullException(nameof(accreditation));

            // set the external id for the accreditation
            accreditation.ExternalId = Guid.NewGuid();
            accreditation.AccreditationStatusId = Common.Enums.AccreditationStatus.Started;

            return await _repository.AddAccreditation(accreditation);
        }

        public async Task<Guid> CreateMaterial(
            Guid externalId,
            Guid? siteId,
            Guid? overseasSiteId,
            AccreditationMaterial accreditationMaterial)
        {
            if (siteId == null &&
                overseasSiteId == null)
                throw new ArgumentNullException("Neither site Id or overseas site Id have been provided");

            if (siteId != null &&
                overseasSiteId != null)
                throw new ArgumentException("Cannopt add a material with both a site Id and overseas site Id");

            accreditationMaterial.ExternalId = Guid.NewGuid();

            return await _repository.AddAccreditationMaterial(
                externalId,
                siteId,
                overseasSiteId,
                accreditationMaterial);
        }

        public async Task UpdateAccreditation(
            Guid externalId,
            DTO.Accreditation accreditation)
        {
            await _repository.UpdateAccreditation(
                externalId,
                accreditation);
        }

        public async Task<DTO.Accreditation> GetAccreditation(Guid externalId)
        {
            var accreditation = await _repository.GetById(externalId);

            return accreditation;
        }

        public async Task<IEnumerable<FileUpload>> GetFileRecords(Guid externalId)
        {
            var fileUploadRecords = await _repository.GetFileRecords(externalId);

            return fileUploadRecords;
        }

        public async Task DeleteFile(
            Guid externalId,
            Guid fileId)
        {
            await _repository.DeleteFile(
                externalId,
                fileId);
        }

        public async Task<IEnumerable<AccreditationTaskProgress>> GetTaskProgress(Guid externalId)
        {
            return await _repository.GetTaskProgress(externalId);
        }

        public async Task AddFile(
            Guid externalId,
            DTO.FileUpload fileUpload)
        {
            await _repository.AddFile(
                externalId, fileUpload);
        }

        public async Task UpdateMaterail(
            Guid externalId,
            Guid? siteId,
            Guid? overseasSiteId,
            Guid materialExternalId,
            AccreditationMaterial accreditationMaterial)
        {
            if (siteId == null &&
                overseasSiteId == null)
                throw new ArgumentNullException("Neither site Id or overseas site Id have been provided");

            if (siteId != null &&
                overseasSiteId != null)
                throw new ArgumentException("Cannopt add a material with both a site Id and overseas site Id");

            await _repository.UpdateMaterial(
                externalId,
                siteId,
                overseasSiteId,
                materialExternalId,
                accreditationMaterial);
        }

        public async Task<AccreditationMaterial> GetMaterial(
            Guid externalId,
            Guid? siteExternalId,
            Guid? overseasSiteExternalId,
            Guid materialExternalId)
        {
            if (siteExternalId == null &&
                overseasSiteExternalId == null)
                throw new ArgumentNullException("Neither site Id or overseas site Id have been provided");

            if (siteExternalId != null &&
                overseasSiteExternalId != null)
                throw new ArgumentException("Cannopt add a material with both a site Id and overseas site Id");

            return await _repository.GetMaterial(
                externalId,
                siteExternalId,
                overseasSiteExternalId,
                materialExternalId);
        }

        public async Task<DTO.Site> GetSite(
            Guid externalId,
            Guid siteExternalId)
        {
            return await _repository.GetSite(externalId, siteExternalId);
        }

        public async Task<Guid> CreateSite(
            Guid externalId,
            Site site)
        {
            return await _repository.CreateSite(
                externalId,
                site);
        }

        public Task UpdateSite(Site site)
        {
            throw new NotImplementedException();
        }

        public async Task<DTO.OverseasReprocessingSite> GetOverseasSite(
            Guid externalId,
            Guid siteExternalId)
        {
            return await _repository.GetOverseasSite(externalId, siteExternalId);
        }

        public async Task<Guid> CreateOverseasSite(
            Guid externalId,
            OverseasReprocessingSite overseasReprocessingSite)
        {
            return await _repository.CreateOverseasSite(
                externalId,
                overseasReprocessingSite);
        }

        public Task UpdateOverseasSite(OverseasReprocessingSite overseasReprocessingSite)
        {
            throw new NotImplementedException();
        }

        public async Task<SaveAndComeBack> GetSaveAndComeBack(Guid externalId)
        {
            return await _repository.GetSaveAndComeBack(externalId);
        }

        public async Task DeleteSaveAndComeBack(Guid externalId)
        {
            await _repository.DeleteSaveAndComeBack(externalId);
        }

        public async Task AddSaveAndComeBack(
            Guid externalId,
            DTO.SaveAndComeBack saveAndComeBack)
        {
            await _repository.AddSaveAndComeBack(externalId, saveAndComeBack);
        }
    }
}
