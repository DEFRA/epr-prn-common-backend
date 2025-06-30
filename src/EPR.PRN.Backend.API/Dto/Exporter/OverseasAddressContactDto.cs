using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace EPR.PRN.Backend.API.Dto.Exporter
{
    public class OverseasAddressContactDto
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
