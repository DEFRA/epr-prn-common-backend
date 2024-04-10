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
            Guid? overseasSiteId,
            AccreditationMaterial accreditationMaterial)
        {
            accreditationMaterial.ExternalId = Guid.NewGuid();

            return await _repository.AddAccreditationMaterial(
                externalId,
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
            Guid? overseasSiteId,
            Guid materialExternalId,
            AccreditationMaterial accreditationMaterial)
        {
            await _repository.UpdateMaterial(
                externalId,
                overseasSiteId,
                materialExternalId,
                accreditationMaterial);
        }

        public async Task<AccreditationMaterial> GetMaterial(
            Guid externalId,
            Guid? overseasSiteExternalId,
            Guid materialExternalId)
        {
            return await _repository.GetMaterial(
                externalId,
                overseasSiteExternalId,
                materialExternalId);
        }

        public async Task<DTO.Site> GetSite(
            Guid externalId)
        {
            return await _repository.GetSite(externalId);
        }

        public async Task<Guid> CreateSite(
            Guid externalId,
            Site site)
        {
            return await _repository.CreateSite(
                externalId,
                site);
        }

        public async Task UpdateSite(
            Guid externalId,
            Site site)
        {
            await _repository.UpdateSite(
                externalId,
                site);
        }

        public async Task<DTO.OverseasReprocessingSite> GetOverseasSite(
            Guid externalId,
            Guid overseasSiteExternalId)
        {
            return await _repository.GetOverseasSite(externalId, overseasSiteExternalId);
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

        public async Task SetOverseasAgent(
            Guid externalId,
            bool? hasOverseasAgent)
        {
            await _repository.SetOverseasAgent(externalId, hasOverseasAgent);
        }
    }
}
