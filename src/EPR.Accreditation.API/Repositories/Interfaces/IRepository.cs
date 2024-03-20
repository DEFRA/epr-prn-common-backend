using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<DTO.Country>> GetCountries();

        Task<Guid> AddAccreditation(DTO.Accreditation accreditation);

        Task<Guid> AddAccreditationMaterial(
            Guid externalId,
            Guid? siteId,
            Guid? overseasSiteId,
            DTO.AccreditationMaterial accreditationMaterial);

        Task AddFile(
            Guid externalId,
            DTO.FileUpload fileUpload);

        Task UpdateAccreditation(
            Guid externalId,
            DTO.Accreditation accreditation);

        Task UpdateMaterial(
            Guid externalId,
            Guid? siteId,
            Guid? overseasSiteId,
            Guid materialExternalId,
            DTO.AccreditationMaterial material);

        Task<DTO.Accreditation> GetById(Guid id);

        Task<IEnumerable<DTO.FileUpload>> GetFileRecords(Guid id);

        Task<IEnumerable<DTO.AccreditationTaskProgress>> GetTaskProgress(Guid externalId);

        Task DeleteFile(
            Guid id,
            Guid fileId);

        Task<DTO.AccreditationMaterial> GetMaterial(
            Guid externalId,
            Guid? siteExternalId,
            Guid? overseasSiteExternalId,
            Guid materialExternalId);

        Task<DTO.SaveAndContinue> GetSaveAndContinue(Guid id);
    }
}
