using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Services.Interfaces
{
    public interface IAccreditationService
    {
        Task<Guid> CreateAccreditation(DTO.Accreditation accreditation);

        Task<Guid> CreateMaterial(
            Guid externalId,
            Guid? overseasSiteId,
            DTO.AccreditationMaterial accreditationMaterial);

        Task AddFile(
            Guid externalId,
            DTO.FileUpload fileUpload);

        Task UpdateAccreditation(
            Guid externalId,
            DTO.Accreditation accreditation);

        Task UpdateMaterail(
            Guid externalId,
            Guid? overseasSiteId,
            Guid materialExternalId,
            DTO.AccreditationMaterial accreditationMaterial);

        Task<DTO.Accreditation> GetAccreditation(Guid externalId);

        Task DeleteFile(
            Guid externalId,
            Guid fileId);

        Task<IEnumerable<DTO.AccreditationTaskProgress>> GetTaskProgress(Guid externalId);

        Task<IEnumerable<DTO.FileUpload>> GetFileRecords(Guid externalId);

        Task<DTO.AccreditationMaterial> GetMaterial(
            Guid externalId,
            Guid? overseasSiteExternalId,
            Guid materialExternalId);

        Task<DTO.Site> GetSite(
            Guid externalId);

        Task<Guid> CreateSite(
            Guid externalId,
            DTO.Site site);

        Task UpdateSite(
            Guid externalId,
            DTO.Site site);

        Task<DTO.OverseasReprocessingSite> GetOverseasSite(
            Guid externalId,
            Guid overseasSiteExternalId);

        Task<Guid> CreateOverseasSite(
            Guid externalId,
            DTO.OverseasReprocessingSite site);

        public Task UpdateOverseasSite(DTO.OverseasReprocessingSite site);

        Task<DTO.SaveAndComeBack> GetSaveAndComeBack(Guid externalId);

        Task DeleteSaveAndComeBack(Guid externalId);

        Task AddSaveAndComeBack(
            Guid externalId,
            DTO.SaveAndComeBack saveAndContinue);

        Task SetOverseasAgent(
            Guid externalId,
            bool? hasOverseasAgent);
    }
}
