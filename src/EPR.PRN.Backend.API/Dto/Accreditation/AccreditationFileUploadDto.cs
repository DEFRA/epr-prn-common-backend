﻿using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Accreditation;

[ExcludeFromCodeCoverage]
public class AccreditationFileUploadDto
{
    public Guid? ExternalId { get; set; }
    public int? OverseasSiteId { get; set; }
    public string Filename { get; set; }
    public Guid? FileId { get; set; }
    public DateTime UploadedOn { get; set; }
    public string UploadedBy { get; set; }
    public int FileUploadTypeId { get; set; }
    public int FileUploadStatusId { get; set; }
}
