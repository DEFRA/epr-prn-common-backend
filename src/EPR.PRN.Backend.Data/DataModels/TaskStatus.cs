using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class TaskStatus
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string? Name { get; set; }
    } 
}
