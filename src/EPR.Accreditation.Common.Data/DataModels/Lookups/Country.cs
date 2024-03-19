using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels.Lookups
{
    [Table("Country", Schema = "Lookup")]
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [MaxLength(2)]
        public string CountryCode { get; set; }

        [MaxLength(200)]
        public string Name {  get; set; }

        #region Navigation properties
        public virtual ICollection<OverseasAddress> OverseasAddresses { get; set; }
        #endregion
    }
}
