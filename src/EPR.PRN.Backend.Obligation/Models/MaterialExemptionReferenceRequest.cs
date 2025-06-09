namespace EPR.PRN.Backend.Obligation.Models
{
    public class MaterialExemptionReferenceRequest
    {
        public Guid ExternalId { get; set; }

        public int RegistrationMaterialId { get; set; }

        public required string ReferenceNumber { get; set; }
    }
}
