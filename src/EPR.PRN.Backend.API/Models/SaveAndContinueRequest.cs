using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Models
{
    [ExcludeFromCodeCoverage]
    public class SaveAndContinueRequest
    {
        public string? Area { get; set; }

        public string? Controller { get; set; }

        public string? Action { get; set; }

        public string? Parameters { get; set; }

        public int RegistrationId { get; set; }
    }
}
