using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class RegistrationStatus
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Registration> Registrations { get; set; }
    }
}