﻿using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto
{
    [ExcludeFromCodeCoverage]
    public class SaveAndContinueDto
    {
        public int Id { get; set; }

        public string? Area { get; set; }

        public string? Controller { get; set; }

        public string? Action { get; set; }

        public string? Parameters { get; set; }

        public int RegistrationId { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
