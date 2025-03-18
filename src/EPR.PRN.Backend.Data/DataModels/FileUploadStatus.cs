using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class FileUploadStatus
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string? Name { get; set; }
    }
}
