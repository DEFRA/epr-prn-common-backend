﻿using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto;

/// <summary>
/// Represents details of a registration material as part of the applicant journey.
/// </summary>
[ExcludeFromCodeCoverage]
public record ApplicantRegistrationMaterialDto
{
    /// <summary>
    /// The unique identifier for the material entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The unique identifier of the registration that this material is registered for.
    /// </summary>
    public Guid RegistrationId { get; set; }

    /// <summary>
    /// Lookup details for the material that this registration is for.
    /// </summary>
    public MaterialLookupDto MaterialLookup { get; set; } = new();

    /// <summary>
    /// Lookup details for the status of the registration of this material.
    /// </summary>
    public MaterialStatusLookupDto StatusLookup { get; set; } = new();

    /// <summary>
    /// A lookup id on the type of permit that has been applied for, for this material registration.
    /// </summary>
    public PermitTypeLookupDto PermitType { get; set; } = new();

    /// <summary>
    /// The ID of the period that the permit applies to, this is only applicable for when the permit type is one of Installation, PPC or environment.
    /// </summary>
    public int? PPCPeriodId { get; set; }

    /// <summary>
    /// The ID of the installation period that the registration material applies to.
    /// </summary>
    public int? InstallationPeriodId { get; set; }

    /// <summary>
    /// The ID of the waste management period that the registration material applies to.
    /// </summary>
    public int? WasteManagementPeriodId { get; set; }

    /// <summary>
    /// The ID of the environmental period that the registration material applies to.
    /// </summary>
    public int? EnvironmentalPeriodId { get; set; }

    /// <summary>
    /// The capacity in tonnes that the reprocessing site can recycle for the material, only applicable if the permit is a PPC permit.
    /// </summary>
    public decimal PPCReprocessingCapacityTonne { get; set; }

    /// <summary>
    /// The capacity in tonnes that the reprocessing site can recycle for the material, only applicable if the permit is a waste management permit.
    /// </summary>
    public decimal WasteManagementReprocessingCapacityTonne { get; set; }

    /// <summary>
    /// The capacity in tonnes that the reprocessing site can recycle for the material, only applicable if the permit is an installation permit.
    /// </summary>
    public decimal InstallationReprocessingTonne { get; set; }

    /// <summary>
    /// The capacity in tonnes that the reprocessing site can recycle for the material, only applicable if the permit is an environmental permit.
    /// </summary>
    public decimal EnvironmentalPermitWasteManagementTonne { get; set; }

    /// <summary>
    /// The maximum capacity in tonnes that the reprocessing site can recycle.
    /// </summary>
    public decimal MaximumReprocessingCapacityTonne { get; set; }

    /// <summary>
    /// The ID of the period that this max weight is applicable within.
    /// </summary>
    public decimal MaximumReprocessingPeriodId { get; set; }

    /// <summary>
    /// The identifying number for the associated PPC permit, only applicable if permit type is PPC.
    /// </summary>
    public string? PPCPermitNumber { get; set; }

    /// <summary>
    /// The identifying number for the associated waste management licence permit, only applicable if permit type is waste management licence.
    /// </summary>
    public string? WasteManagementLicenceNumber { get; set; }

    /// <summary>
    /// The identifying number for the associated installation permit, only applicable if permit type is installation.
    /// </summary>
    public string? InstallationPermitNumber { get; set; }

    /// <summary>
    /// The identifying number for the associated environmental permit, only applicable if permit type is environmental.
    /// </summary>
    public string? EnvironmentalPermitWasteManagementNumber { get; set; }

    /// <summary>
    /// Flag to determine if the material is being registered for as part of the overall registration application.
    /// </summary>
    public bool IsMaterialRegistered { get; set; }

    /// <summary>
    /// Collection of associated exemption references for the material.
    /// </summary>
    public List<ExemptionReferencesLookupDto> ExemptionReferences { get; set; } = new();
}