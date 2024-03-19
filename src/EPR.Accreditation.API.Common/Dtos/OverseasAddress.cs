using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Dtos
{
    public class OverseasAddress
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }
    }
}
