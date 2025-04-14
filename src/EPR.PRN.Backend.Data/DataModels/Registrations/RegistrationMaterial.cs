﻿using EPR.PRN.Backend.API.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

public class RegistrationMaterial
{
    [Key]
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public Registration Registration { get; set; }
    [ForeignKey("Registration")]
    public int RegistrationId { get; set; }
    public LookupMaterial Material { get; set; }
    [ForeignKey("Material")]
    public int MaterialId { get; set; }
    public int PermitId { get; set; }
    public LookupRegistrationMaterialStatus Status { get; set; }
    [ForeignKey("Status")]
    public int StatusID { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;        
    public decimal MaximumProcessingCapacityTonnes { get; set; }
    public DateTime DeterminationDate { get; set; }
    public DateTime ProcessingStartDate { get; set; }
    public DateTime ProcessingEndDate { get; set; }
    public DateTime StatusUpdatedDate { get; set; }
}