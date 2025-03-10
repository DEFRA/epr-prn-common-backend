using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class RegistrationTaskStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        public int RegistrationContactId { get; set; }

        public int TaskId { get; set; }

        public int TaskStatusId { get; set; }

        public virtual TaskStatus TaskStatus { get; set; }

        public virtual TaskName Task { get; set; }

        public virtual RegistrationContact RegistrationContact { get; set; }

    }
}
