using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class Material
    {
        [Key]
        [MaxLength(20)]
        public required string MaterialName { get; set; }

        [MaxLength(3)]
        public required string MaterialCode { get; set; }

        public virtual ICollection<Registration> Registrations { get; set; } = null!;
        public virtual ICollection<RegistrationMaterial> RegistrationMaterials { get; set; } = null!;
        public virtual ICollection<RegistrationReprocessingIO> RegistrationReprocessingIOs { get; set; } = null!;
        public virtual ICollection<RegistrationContact> RegistrationContacts { get; set; } = null!;
    }
}
