namespace EPR.PRN.Backend.API.Common.Constants;

/// <summary>
/// Contains the names of the tasks that are applicable to the exporter and reprocessor application types, for both registration and accreditation journey types.
/// This is kept separate from the regulator tasks to ensure isolation and easier maintenance in future in line with ADR 112 and 112A.
/// </summary>
public static class ApplicantRegistrationTaskNames
{
    public const string AccreditationSamplingAndInspectionPlan = "AccreditationSamplingAndInspectionPlan";
    public const string BusinessPlan = "BusinessPlan";
    public const string BusinessAddress = "BusinessAddress";
    public const string OverseasReprocessingSitesAndBroadlyEquivalentEvidence = "OverseasReprocessingSitesAndEvidenceOfBroadlyEquivalentStandards";
    public const string PrnsTonnageAndAuthorityToIssuePrns = "PRNsTonnageAndAuthorityToIssuePRNs";
    public const string PERNsTonnageAndAuthorityToIssuePERNs = "PERNsTonnageAndAuthorityToIssuePERNs";
    public const string ReprocessingInputsAndOutputs = "ReprocessingInputsAndOutputs";
    public const string SamplingAndInspectionPlan = "SamplingAndInspectionPlan";
    public const string SiteAddressAndContactDetails = "SiteAddressAndContactDetails";
    public const string WasteLicensesPermitsAndExemptions = "WasteLicensesPermitsAndExemptions";
    public const string WasteCarrierBrokerDealerNumber = "WasteCarrierBrokerDealerNumber";
    public const string OverseasReprocessorSiteDetails = "OverseasReprocessingSites";
    public const string InterimSites = "InterimSites";
}