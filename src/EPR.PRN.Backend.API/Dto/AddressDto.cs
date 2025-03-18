using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto
{
    [ExcludeFromCodeCoverage]
    public class AddressDto
    {
        public string AddressType { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;

        public string AddressLine2 { get; set; } = null!;

        public string TownCity { get; set; } = null!;

        public string County { get; set; } = null!;

        public string Postcode { get; set; } = null!;

        public int NationId { get; set; } 

        public string GridReference { get; set; } = null!;
    }
}
