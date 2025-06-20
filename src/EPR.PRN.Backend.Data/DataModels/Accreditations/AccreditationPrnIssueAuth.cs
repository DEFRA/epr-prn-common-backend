﻿namespace EPR.PRN.Backend.Data.DataModels.Accreditations;

public class AccreditationPrnIssueAuth
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid AccreditationExternalId { get; set; }
    public int AccreditationId { get; set; }
    public Guid PersonExternalId { get; set; }
    public AccreditationEntity? Accreditation { get; set; }
}