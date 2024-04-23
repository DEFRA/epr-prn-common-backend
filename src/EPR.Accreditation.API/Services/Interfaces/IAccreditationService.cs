using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Services.Interfaces
{
    public interface IAccreditationService
    {
        Task<Guid> CreateAccreditation(DTO.Accreditation accreditation);

        Task<Guid> CreateMaterial(
            Guid id,
            Guid? overseasSiteId,
            DTO.AccreditationMaterial accreditationMaterial);

        Task AddFile(
            Guid id,
            DTO.FileUpload fileUpload);

        Task UpdateAccreditation(
            Guid id,
            DTO.Accreditation accreditation);

        Task UpdateMaterail(
            Guid id,
            Guid? overseasSiteId,
            Guid materialid,
            DTO.AccreditationMaterial accreditationMaterial);

        Task<DTO.Accreditation> GetAccreditation(Guid id);

        Task DeleteFile(
            Guid id,
            Guid fileId);

        Task<IEnumerable<DTO.AccreditationTaskProgress>> GetTaskProgress(Guid id);

        Task<IEnumerable<DTO.FileUpload>> GetFileRecords(Guid id);

        Task<DTO.AccreditationMaterial> GetMaterial(
            Guid id,
            Guid? overseasSiteid,
            Guid materialid);

        Task<DTO.Site> GetSite(
            Guid id);

        Task<Guid> CreateSite(
            Guid id,
            DTO.Site site);

        Task UpdateSite(
            Guid id,
            DTO.Site site);

        Task<DTO.OverseasReprocessingSite> GetOverseasSite(
            Guid id,
            Guid overseasSiteid);

        Task<Guid> CreateOverseasSite(
            Guid id,
            DTO.OverseasReprocessingSite site);

        public Task UpdateOverseasSite(
            Guid id,
            DTO.OverseasReprocessingSite site);

        Task<DTO.SaveAndComeBack> GetSaveAndComeBack(Guid id);

        Task DeleteSaveAndComeBack(Guid id);

        Task AddSaveAndComeBack(
            Guid id,
            DTO.SaveAndComeBack saveAndContinue);
    }
}
