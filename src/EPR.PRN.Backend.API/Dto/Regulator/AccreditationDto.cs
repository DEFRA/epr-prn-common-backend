using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator
{
    [ExcludeFromCodeCoverage]
    public class AccreditationDto
    {
        public Guid Id { get; set; } 
        public int RegistrationMaterialId { get; set; }
        public string? Status { get; set; }
        public int AccreditationYear { get; set; }
        public string? ApplicationReference { get; set; }
        public DateTime? DeterminationDate { get; set; }
        public List<RegistrationTaskDto> Tasks { get; set; } = [];
    }
}