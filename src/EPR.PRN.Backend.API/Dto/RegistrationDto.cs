using EPR.PRN.Backend.Data.DataModels;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto
{
    [ExcludeFromCodeCoverage]
    public class RegistrationDto
    {
        public int ApplicationTypeId { get; set; }
        public Guid OrganisatonId { get; set; }
        public int RegistrationStatusId { get; set; }
        public string RegistrationStatus { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        public List<AddressDto> Addresses { get; set; } = null!;

    }
}
