using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class MaterialPaymentFeeDto
{
    public int OrganisationId { get; set; }
    public string OrganisationName { get; set; }
    public string SiteAddress { get; set; } = string.Empty;
    public string PaymentReference { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
