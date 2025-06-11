namespace EPR.PRN.Backend.Data.DTO.Registration
{

    public class RegistrationOverviewDto

    {
        public Guid RegistrationId { get; set; }

        public int RegistrationMaterialId { get; set; }

        public int MaterialId { get; set; }

        public string? Material { get; set; }

        public int ApplicationTypeId { get; set; }

        public string? ApplicationType { get; set; }

        public int RegistrationStatus { get; set; }

        public int AccreditationStatus { get; set; }

        public int? ReprocessingSiteId { get; set; }

        public AddressDto? ReprocessingSiteAddress { get; set; }

        public int Year { get; set; }
    }
}
