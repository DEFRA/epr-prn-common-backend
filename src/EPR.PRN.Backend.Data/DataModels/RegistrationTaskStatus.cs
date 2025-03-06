using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class RegistrationTaskStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        public int RegistrationId { get; set; }

        public int TaskId { get; set; }

        public int TaskStatusId { get; set; }

        public virtual TaskStatus TaskStatus { get; set; } = null!;
        public virtual TaskName Task { get; set; } = null!;

        public virtual Registration Registration { get; set; } = null!;

    }
}
