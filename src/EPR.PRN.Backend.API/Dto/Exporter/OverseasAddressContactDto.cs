using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace EPR.PRN.Backend.API.Dto.Exporter
{
    public class OverseasAddressContactDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
