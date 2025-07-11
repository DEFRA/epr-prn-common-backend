namespace EPR.PRN.Backend.API.Common.Constants;

/// <summary>
/// Contains the names of the tasks that are applicable to the exporter and reprocessor application types, for both registration and accreditation journey types.
/// This is kept separate from the regulator tasks to ensure isolation and easier maintenance in future in line with ADR 112 and 112A.
/// </summary>
public static class ApplicantRegistrationTaskNames
{
    //public const string AccreditationSamplingAndInspectionPlan = "AccreditationSamplingAndInspectionPlan";                                                //NOSONAR
    //public const string BusinessPlan = "BusinessPlan";                                                                                                    //NOSONAR
    //public const string BusinessAddress = "BusinessAddress";                                                                                              //NOSONAR
    //public const string OverseasReprocessingSitesAndBroadlyEquivalentEvidence = "OverseasReprocessingSitesAndEvidenceOfBroadlyEquivalentStandards";       //NOSONAR
    //public const string PrnsTonnageAndAuthorityToIssuePrns = "PRNsTonnageAndAuthorityToIssuePRNs";                                                        //NOSONAR
    //public const string PERNsTonnageAndAuthorityToIssuePERNs = "PERNsTonnageAndAuthorityToIssuePERNs";                                                    //NOSONAR
    //public const string ReprocessingInputsAndOutputs = "ReprocessingInputsAndOutputs";                                                                    //NOSONAR

    public const string SiteAddressAndContactDetails = "SiteAddressAndContactDetails";                              // Reprocessor
    public const string WasteLicensesPermitsAndExemptions = "WasteLicensesPermitsAndExemption";                     // Reprocessor
    public const string AboutthePackagingYouAreRegistering = "AboutthePackagingYouAreRegistering";                  // Reprocessor
    public const string SamplingAndInspectionPlan = "SamplingAndInspectionPlan";                                    // Reprocessor & Exporter

    public const string AddressForServiceofNotices = "AddressForServiceofNotices";                                  // Exporter
    public const string CarrierBrokerDealerNumberAndOtherPermits = "CarrierBrokerDealerNumberAndOtherPermits";      // Exporter
    public const string AboutThePackagingWasteYouExport = "AboutThePackagingWasteYouExport";                        // Exporter
    public const string OverseasReprocessingSitesYouExportTo = "OverseasReprocessingSitesYouExportTo";              // Exporter
    public const string InterimSites = "InterimSites";                                                              // Exporter


}