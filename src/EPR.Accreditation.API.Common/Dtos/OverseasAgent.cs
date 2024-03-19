using System.ComponentModel.DataAnnotations;

namespace EPR.Accreditation.API.Common.Dtos
{
    public class OverseasAgent
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Fullname { get; set; }

        [MaxLength(100)]
        public string Position { get; set; }

        [MaxLength(30)]
        public string Telephone { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public OverseasAddress OverseasAddress { get; set; }
    }
}
