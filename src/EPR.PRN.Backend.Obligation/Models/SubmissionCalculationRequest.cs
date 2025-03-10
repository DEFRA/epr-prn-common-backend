﻿#nullable disable

namespace EPR.PRN.Backend.Obligation.Models;

public class SubmissionCalculationRequest
{
    public Guid SubmissionId { get; set; }
    public string SubmissionPeriod { get; set; }
    public string PackagingMaterial { get; set; }
    public int PackagingMaterialWeight { get; set; }
}
