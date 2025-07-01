using System.Reflection.Metadata.Ecma335;

namespace EPR.PRN.Backend.API.Dto.Exporter
{
    public class OverseasAddressWasteCodesDto
    {
        public Guid ExternalId { get; set; }        
        public required string CodeName { get; set; }
    }
}
