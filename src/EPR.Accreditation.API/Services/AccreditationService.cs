using EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Helpers;
using EPR.Accreditation.API.Helpers.Comparers;
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
            Guid id,
            Guid? overseasSiteId,
            AccreditationMaterial accreditationMaterial)
        {
            return await _repository.AddAccreditationMaterial(
                id,
                overseasSiteId,
                accreditationMaterial);
        }

        public async Task UpdateAccreditation(
            Guid id,
            DTO.Accreditation accreditation)
        {
            await _repository.UpdateAccreditation(
                id,
                accreditation);
        }

        public async Task<DTO.Accreditation> GetAccreditation(Guid id)
        {
            var accreditation = await _repository.GetAccreditation(id);

            return accreditation;
        }

        public async Task<IEnumerable<FileUpload>> GetFileRecords(Guid id)
        {
            var fileUploadRecords = await _repository.GetFileRecords(id);

            return fileUploadRecords;
        }

        public async Task DeleteFile(
            Guid id,
            Guid fileId)
        {
            await _repository.DeleteFile(
                id,
                fileId);
        }

        public async Task<IEnumerable<AccreditationTaskProgress>> GetTaskProgress(Guid id)
        {
            return await _repository.GetTaskProgress(id);
        }

        public async Task AddFile(
            Guid id,
            DTO.FileUpload fileUpload)
        {
            await _repository.AddFile(
                id, fileUpload);
        }

        public async Task UpdateMaterail(
            Guid id,
            Guid? overseasSiteId,
            Guid materialid,
            AccreditationMaterial accreditationMaterial)
        {
            if (accreditationMaterial.WasteCodes != null &&
                accreditationMaterial.WasteCodes.Any())
            {
                accreditationMaterial.WasteCodes =
                    accreditationMaterial
                        .WasteCodes
                        .Where(wc => !string.IsNullOrWhiteSpace(wc.Code))
                        .Distinct(new WasteCodeDtoComparer());
            }

            await _repository.UpdateMaterial(
                id,
                overseasSiteId,
                materialid,
                accreditationMaterial);
        }

        public async Task<AccreditationMaterial> GetMaterial(
            Guid id,
            Guid? overseasSiteid,
            Guid materialid)
        {
            return await _repository.GetMaterial(
                id,
                overseasSiteid,
                materialid);
        }

        public async Task<DTO.Site> GetSite(
            Guid id)
        {
            return await _repository.GetSite(id);
        }

        public async Task<Guid> CreateSite(
            Guid id,
            Site site)
        {
            // ensure accreditation is an exporter
            var accreditation = await _repository.GetAccreditation(id);

            if (accreditation == null)
            {
                throw new NotFoundException($"No accreditation found with ID: {id}");
            }

            if (accreditation.OperatorTypeId != Common.Enums.OperatorType.Reprocessor)
            {
                throw new InvalidOperationException($"Cannot add Overseas Site to Reprocessor Accreditation. Id: {id}");
            }

            return await _repository.CreateSite(
                id,
                site);
        }

        public async Task UpdateSite(
            Guid id,
            Site site)
        {
            await _repository.UpdateSite(
                id,
                site);
        }

        public async Task<DTO.OverseasReprocessingSite> GetOverseasSite(
            Guid id,
            Guid overseasSiteid)
        {
            return await _repository.GetOverseasSite(id, overseasSiteid);
        }

        public async Task<Guid> CreateOverseasSite(
            Guid id,
            OverseasReprocessingSite overseasReprocessingSite)
        {
            return await _repository.CreateOverseasSite(
                id,
                overseasReprocessingSite);
        }

        public async Task UpdateOverseasSite(
            Guid id,
            Guid overseasSiteId,
            OverseasReprocessingSite overseasReprocessingSite)
        {
            await _repository.UpdateOverseasSite(
                id,
                overseasSiteId,
                overseasReprocessingSite);
        }

        public async Task<SaveAndComeBack> GetSaveAndComeBack(Guid id)
        {
            return await _repository.GetSaveAndComeBack(id);
        }

        public async Task DeleteSaveAndComeBack(Guid id)
        {
            await _repository.DeleteSaveAndComeBack(id);
        }

        public async Task AddSaveAndComeBack(
            Guid id,
            DTO.SaveAndComeBack saveAndComeBack)
        {
            await _repository.AddSaveAndComeBack(id, saveAndComeBack);
        }
    }
}
