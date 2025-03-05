using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class FileUpload
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }
       
        public int RegistrationId { get; set; }
       
        public string? FileName { get; set; }
       
        public Guid FileId { get; set; }
        
        public DateTime DateUploaded { get; set; }
       
        public required Guid UploadedBy { get; set; }
        
        public required string FileUploadTypeId { get; set; }
        
        public int MaterialId { get; set; }
        
        public int FileUploadStatus { get; set; }
    }
}
