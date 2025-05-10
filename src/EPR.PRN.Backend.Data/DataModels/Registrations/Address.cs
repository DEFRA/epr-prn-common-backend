using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.Address")]
[ExcludeFromCodeCoverage]
public class Address
{
    public int Id { get; set; }
    [MaxLength(200)]
    public string AddressLine1 { get; set; }
    [MaxLength(200)]
    public string AddressLine2 { get; set; }
    [MaxLength(70)]
    public string TownCity { get; set; }
    [MaxLength(50)]
    public string? County { get; set; }
    [MaxLength(10)]
    public string PostCode { get; set; }
    public int? NationId { get; set; }
    [MaxLength(20)]
    public string GridReference { get; set; }
    public string? Nation
    {
        get
        {
            switch (NationId)
            {
                case 1:
                    return "England";
                case 2:
                    return "Scotland";
                case 3:
                    return "Wales";
                case 4:
                    return "Ireland";
                default:
                    return null;
            }
        }
    }
}