using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class Period
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [InverseProperty(nameof(RegistrationMaterial.MaxPeriod))]
        public virtual ICollection<RegistrationMaterial> MaxPeriods { get; set; } = null!;

        [InverseProperty(nameof(RegistrationMaterial.InstallationPeriod))]
        public virtual ICollection<RegistrationMaterial> InstallationPeriods { get; set; } = null!;

        [InverseProperty(nameof(RegistrationMaterial.EnvironmentalPermitWasteManagementPeriod))]
        public virtual ICollection<RegistrationMaterial> EnvironmentalPermitWasteManagementPeriods { get; set; } = null!;

        [InverseProperty(nameof(RegistrationMaterial.WasteManagementPeriod))]
        public virtual ICollection<RegistrationMaterial> WasteManagementPeriods { get; set; } = null!;

        [InverseProperty(nameof(RegistrationMaterial.PPCPeriod))]
        public virtual ICollection<RegistrationMaterial> PPCPeriods { get; set; } = null!;

        [InverseProperty(nameof(RegistrationMaterial.MaximumReprocessingPeriod))]
        public virtual ICollection<RegistrationMaterial> MaximumReprocessingPeriods { get; set; } = null!;
    }
}