namespace EPR.PRN.Backend.API.Dto.Regulator
{
    public class AccreditationDto
    {
        public int Id { get; set; } 
        public int RegistrationMaterialId { get; set; }
        public string? Status { get; set; }
        public int AccreditationYear { get; init; }
        public List<RegistrationTaskDto> Tasks { get; set; } = [];
    }
}