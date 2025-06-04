namespace EPR.PRN.Backend.API.Dto.Regulator
{
    public class RegistrationMaterialSamplingPlanDto
    {
        public required string MaterialName { get; set; }
        public List<RegistrationMaterialSamplingPlanFileDto> Files { get; set; } = [];
    }
}