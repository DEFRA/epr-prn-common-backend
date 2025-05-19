using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator
{
    [ExcludeFromCodeCoverage]
    public class AccreditationRegistrationTaskDto : RegistrationTaskDto
    {
        public int AccreditationYear { get; set; }
    }
}