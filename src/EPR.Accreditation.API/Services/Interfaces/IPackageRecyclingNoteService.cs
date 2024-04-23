using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Services.Interfaces
{
    public interface IPackageRecyclingNoteService
    {
        /// <summary>
        /// Retrieve the data for a PRN.
        /// </summary>
        /// <param name="externalId">The ID of the PRN to retrieve data for.</param>
        /// <returns>A <see cref="DTO.PackageRecyclingNoteResponse"/> object.</returns>
        Task<DTO.PackageRecyclingNoteResponse> GetPackageRecyclingNote(Guid externalId);

        /// <summary>
        /// Store a new PRN record.
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        Task<Guid> CreatePackageRecyclingNote(DTO.PackageRecyclingNoteRequest prn);

        Task UpdatePrn(Guid prnId, DTO.PrnUpdateRequest newData);

        /// <summary>
        /// Retrieve a list of PRNs for the specified organisation.
        /// </summary>
        /// <param name="organisationId">The ID of the organisation to retrieve the PRNs for.</param>
        /// <returns>An <see cref="IEnumerable{Guid}"/> object.</returns>
        Task<IEnumerable<Guid>> GetPrnsForOrganisation(Guid organisationId);

        /// <summary>
        /// Update the status of a PRN.
        /// </summary>
        /// <param name="status">The details of the status update.</param>
        Task UpdatePrnStatus(Guid prnId, DTO.PrnStatusHistoryRequest status);

        /// <summary>
        /// Delete a PRN.
        /// </summary>
        /// <param name="prnId">The ID of the PRN to delete.</param>
        Task DeletePrn(Guid prnId);

    }
}
