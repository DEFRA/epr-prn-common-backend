using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Dtos
{
    public class OverseasContactPerson
    {
        public int OverseasAddressId { get; set; }

        public int OverseasReprocessingSiteId { get; set; }

        [MaxLength(100)]
        public string Fullname { get; set; }

        [MaxLength(100)]
        public string Position { get; set; }

        [MaxLength(30)]
        public string Telephone { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public Enums.OverseasPersonType OverseasPersonTypeId { get; set; }

        public virtual OverseasAddress OverseasAddress { get; set; }

        public virtual OverseasReprocessingSite OverseasReprocessingSite { get; set; }
    }
}
