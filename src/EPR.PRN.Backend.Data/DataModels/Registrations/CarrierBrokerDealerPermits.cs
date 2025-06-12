using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.CarrierBrokerDealerPermits")]
[ExcludeFromCodeCoverage]
public class CarrierBrokerDealerPermit
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ExternalId { get; set; }
    public int RegistrationId { get; set; }
    [MaxLength(20), AllowNull]
    [Column(TypeName = "varchar(20)")]
    public string WasteCarrierBrokerDealerRegistrstion { get; set; }
    [MaxLength(20), AllowNull]
    [Column(TypeName = "varchar(20)")]
    public string WasteManagementorEnvironmentPermitNumber { get; set; }
    [MaxLength(20), AllowNull]
    [Column(TypeName = "varchar(20)")]
    public string InstallationPermitorPPCNumber { get; set; }
    [MaxLength(150), AllowNull]
    [Column(TypeName = "varchar(150)")]
    public string WasteExemptionReference { get; set; }
    public bool RegisteredWasteCarrierBrokerDealerFlag { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}