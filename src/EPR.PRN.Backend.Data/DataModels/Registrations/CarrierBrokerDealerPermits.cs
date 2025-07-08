using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.CarrierBrokerDealerPermits")]
[ExcludeFromCodeCoverage]
public class CarrierBrokerDealerPermits
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ExternalId { get; set; }

    public Registration Registration { get; set; } = null!; // Navigation property to Registration

    public int RegistrationId { get; set; }

    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    public string? WasteCarrierBrokerDealerRegistration { get; set; }
    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    public string? WasteManagementEnvironmentPermitNumber { get; set; }
    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    public string? InstallationPermitOrPPCNumber { get; set; }
    [MaxLength(150)]
    [Column(TypeName = "varchar(150)")]
    public string? WasteExemptionReference { get; set; }

    public bool RegisteredWasteCarrierBrokerDealerFlag { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}