using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class Period
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [InverseProperty(nameof(RegistrationMaterial.MaxPeriod))]
        public virtual ICollection<RegistrationMaterial> MaxPeriods { get; set; }

        [InverseProperty(nameof(RegistrationMaterial.InstallationPeriod))]
        public virtual ICollection<RegistrationMaterial> InstallationPeriods { get; set; }

        [InverseProperty(nameof(RegistrationMaterial.EnvironmentalPermitWasteManagementPeriod))]
        public virtual ICollection<RegistrationMaterial> EnvironmentalPermitWasteManagementPeriods { get; set; }

        [InverseProperty(nameof(RegistrationMaterial.WasteManagementPeriod))]
        public virtual ICollection<RegistrationMaterial> WasteManagementPeriods { get; set; }

        [InverseProperty(nameof(RegistrationMaterial.PPCPeriod))]
        public virtual ICollection<RegistrationMaterial> PPCPeriods { get; set; }

        [InverseProperty(nameof(RegistrationMaterial.MaximumReprocessingPeriod))]
        public virtual ICollection<RegistrationMaterial> MaximumReprocessingPeriods { get; set; }
    }
}