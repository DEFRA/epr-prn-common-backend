using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Services.Interfaces
{
    public interface IAccreditationService
    {
        Task<Guid> CreateAccreditation(DTO.Accreditation accreditation);

        Task<Guid> CreateMaterial(
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

        Task UpdateMaterail(
            Guid externalId,
            Guid? siteId,
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
            Guid? siteExternalId,
            Guid? overseasSiteExternalId,
            Guid materialExternalId);

        Task<DTO.SaveAndContinue> GetSaveAndContinue(Guid externalId);

        Task DeleteSaveAndContinue(Guid externalId);
    }
}
