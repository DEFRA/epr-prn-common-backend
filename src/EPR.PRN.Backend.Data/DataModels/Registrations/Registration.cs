using System.ComponentModel.DataAnnotations;
namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }                           // Unique identifier for each registration record
        public string ExternalId { get; set; }                  // GUID for external reference
        public int ApplicationTypeId { get; set; }            // Identifier for the type of application
        public int OrganisationId { get; set; }                // Identifier for the organization
        public int RegistrationStatusId { get; set; }         // Status of the registration
        public int BusinessAddressId { get; set; }            // Identifier for the business address
        public int ReprocessingSiteAddressId { get; set; }    // Identifier for the reprocessing site address
        public int LegalDocumentAddressId { get; set; }       // Identifier for the legal document address
        public int AssignedOfficerId { get; set; }            // Identifier for the officer assigned to the registration
        public Guid CreatedBy { get; set; }                    // User who created the registration record
        public DateTime CreatedDate { get; set; }              // Date and time when the record was created
        public Guid UpdatedBy { get; set; }                    // User who last updated the record
        public DateTime? UpdatedDate { get; set; }             // Date and time when the record was last updated
    }
}