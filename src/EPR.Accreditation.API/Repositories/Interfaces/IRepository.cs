using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<DTO.Country>> GetCountries();

        Task<Guid> AddAccreditation(DTO.Accreditation accreditation);

        Task<Guid> AddAccreditationMaterial(
            Guid externalId,
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
            Guid? overseasSiteExternalId,
            Guid materialExternalId);

        Task<DTO.Site> GetSite(
            Guid id);

        Task<Guid> CreateSite(
            Guid externalId,
            DTO.Site site);

        Task UpdateSite(
            Guid externalId,
            DTO.Site site);

        Task<DTO.OverseasReprocessingSite> GetOverseasSite(
            Guid id,
            Guid siteExternalId);

        Task<Guid> CreateOverseasSite(
            Guid id,
            DTO.OverseasReprocessingSite site);

        Task UpdateOverseasSite(DTO.OverseasReprocessingSite site);

        Task<DTO.SaveAndComeBack> GetSaveAndComeBack(Guid id);

        Task DeleteSaveAndComeBack(Guid id);

        Task AddSaveAndComeBack(
            Guid externalId,
            DTO.SaveAndComeBack saveAndContinue);

        Task<IEnumerable<DTO.Material>> GetMaterials();

        #region PRN Management

        /// <summary>
        /// Saves a new PRN to the database.
        /// </summary>
        /// <param name="prn">The PRN to save.</param>
        /// <returns>The GUID that identifies the PRN.</returns>
        Task<Guid> AddPackageRecyclingNote(DTO.PackageRecyclingNoteRequest prn);

        /// <summary>
        /// Updates the data of an existing PRN.
        /// </summary>
        /// <param name="prnId"></param>
        /// <param name="update">The new data to update the record with.</param>
        /// <returns></returns>
        Task UpdatePrn(Guid prnId, DTO.PackageRecyclingNoteRequest update);

        /// <summary>
        /// Retrieves details of a PRN from the database.
        /// </summary>
        /// <param name="id">The GUID of the PRN to retrieve.</param>
        /// <returns>A <see cref="DTO.PackageRecyclingNoteResponse"/> object.</returns>
        Task<DTO.PackageRecyclingNoteResponse>GetPackageRecyclingNote(Guid id);

        /// <summary>
        /// Retrieves a list of the PRNs associated with the specified Organisation
        /// </summary>
        /// <param name="organisationId">The ID of the organisation.</param>
        /// <returns>An enumerable of PRN IDs.</returns>
        Task<IEnumerable<Guid>> GetPrnsForOrganisation(Guid organisationId);

        /// <summary>
        /// Updates the status of a PRN.
        /// </summary>
        /// <param name = "status" > The details of the status update.</param>
        Task UpdatePrnStatus(Guid prnId, DTO.PrnStatusHistoryRequest status);

        /// <summary>
        /// Deletes a PRN.
        /// </summary>
        /// <param name="id">The ID of the PRN to delete.</param>
        Task DeletePrn(Guid id);
        #endregion
    }
}
