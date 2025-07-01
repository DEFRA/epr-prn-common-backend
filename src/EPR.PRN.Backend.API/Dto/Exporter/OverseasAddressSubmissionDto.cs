namespace EPR.PRN.Backend.API.Dto.Exporter
{
    public class OverseasAddressSubmissionDto
    {
        List<OverseasAddressDto> OverseasAddresses { get; set; } = [];
        public Guid RegistrationMaterialId { get; set; }
    }
}
