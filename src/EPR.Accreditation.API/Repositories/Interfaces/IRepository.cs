using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<DTO.Country>> GetCountries();

        Task<DTO.Accreditation> GetAccreditation(Guid id);

        Task<Guid> AddAccreditation(DTO.Accreditation accreditation);

        Task<Guid> AddAccreditationMaterial(
            Guid id,
            Guid? overseasSiteId,
            DTO.AccreditationMaterial accreditationMaterial);

        Task AddFile(
            Guid id,
            DTO.FileUpload fileUpload);

        Task UpdateAccreditation(
            Guid id,
            DTO.Accreditation accreditation);

        Task UpdateMaterial(
            Guid id,
            Guid? overseasSiteId,
            Guid materialid,
            DTO.AccreditationMaterial material);

        Task<IEnumerable<DTO.FileUpload>> GetFileRecords(Guid id);

        Task<IEnumerable<DTO.AccreditationTaskProgress>> GetTaskProgress(Guid id);

        Task DeleteFile(
            Guid id,
            Guid fileId);

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
            Guid siteid);

        Task<Guid> CreateOverseasSite(
            Guid id,
            DTO.OverseasReprocessingSite site);

        Task UpdateOverseasSite(
            Guid id,
            Guid overseasSiteId,
            DTO.OverseasReprocessingSite site);

        Task<DTO.SaveAndComeBack> GetSaveAndComeBack(Guid id);

        Task DeleteSaveAndComeBack(Guid id);

        Task AddSaveAndComeBack(
            Guid id,
            DTO.SaveAndComeBack saveAndContinue);

        Task<IEnumerable<DTO.Material>> GetMaterials();
    }
}
