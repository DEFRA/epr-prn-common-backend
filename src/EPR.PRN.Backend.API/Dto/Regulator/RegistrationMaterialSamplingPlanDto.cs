﻿using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Regulator
{
    [ExcludeFromCodeCoverage]
    public class RegistrationMaterialSamplingPlanDto:NoteBase
    {
        public required string MaterialName { get; set; }
        public List<RegistrationMaterialSamplingPlanFileDto> Files { get; set; } = [];
        public Guid RegistrationId { get; set; }
        public Guid RegistrationMaterialId { get; set; }
        public string SiteAddress { get; set; } = string.Empty;
        public Guid RegulatorApplicationTaskStatusId { get; set; }
    }
}