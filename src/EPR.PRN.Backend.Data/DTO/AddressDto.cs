using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DTO;

[ExcludeFromCodeCoverage]
public record AddressDto
{
    public int? Id { get; set; }
    
    public string AddressLine1 { get; set; } = string.Empty;
    
    public string? AddressLine2 { get; set; } = string.Empty;

    public string TownCity { get; set; } = string.Empty;
    
    public string? County { get; set; }
    
    public string? Country { get; set; } = string.Empty;
    
    public string PostCode { get; set; } = string.Empty;
    
    public int? NationId { get; set; }   

    public string? GridReference { get; set; } = string.Empty;
}
