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

    public int RegistrationId { get; set; }

    public string? WasteCarrierBrokerDealerRegistration { get; set; }

    public string? WasteManagementEnvironmentPermitNumber { get; set; }

    public string? InstatallationPermitOrPPCNumber { get; set; }

    public bool RegisteredWasteCarrierBrokerDealerFlag { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}