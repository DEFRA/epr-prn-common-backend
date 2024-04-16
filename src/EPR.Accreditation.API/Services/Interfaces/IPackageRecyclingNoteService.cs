using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Services.Interfaces
{
    public interface IPackageRecyclingNoteService
    {
        Task<DTO.PackageRecyclingNote> GetPackageRecyclingNote(Guid externalId);
        Task<Guid> CreatePackageRecyclingNote(DTO.PackageRecyclingNote prn);

    }
}
