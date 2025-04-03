using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
	public class RegistrationMaterial
    {
        [Key]
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public int RegistrationId { get; set; }
        public int MaterialId { get; set; }
        public int PermitId { get; set; }
        public int StatusID { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;        
        public decimal MaximumProcessingCapacityTonnes { get; set; }
        public DateTime DeterminationDate { get; set; }
        public DateTime ProcessingStartDate { get; set; }
        public DateTime ProcessingEndDate { get; set; }
        public DateTime StatusUpdatedDate { get; set; }
    }

    
   
}