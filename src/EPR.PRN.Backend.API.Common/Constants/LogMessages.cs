namespace EPR.PRN.Backend.API.Common.Constants;

public static class LogMessages
{
    public const string RegistrationMaterialsTasks = "Attempting to get registration with materials and tasks";
    public const string AccreditationMaterialsTasks = "Attempting to get registration with materials, accreditations and tasks";
    public const string RegistrationAccreditationsMaterialsTasks = "Attempting to get registration accreditations with materials and tasks";
    public const string SummaryInfoMaterial = "Attempting to get summary info for a material";
    public const string OutcomeMaterialRegistration = "UpdateRegistrationOutcome called with Id: {Id}";
    public const string RegistrationSiteAddress = "Attempting to get site address info for registration id :{Id}";
    public const string RegistrationWasteCarrier = "Attempting to get waste carrier details for registration id :{Id}";
    public const string MaterialAuthorization = "Attempting to get material authorisation  site of registration id :{Id}";
    public const string RegistrationMaterialpaymentFees = "Attempting to get registration material payment fee by id :{Id}";
    public const string UpdateRegulatorApplicationTask = "UpdateRegulatorApplicationTask";
    public const string UpdateRegulatorRegistrationTask = "UpdateRegulatorRegistrationTask";
    public const string UpdateRegulatorAccreditationTask = "UpdateRegulatorAccreditationTask";
    public const string AccreditationSamplingPlan = "Attempting to get file uploads relating to an accreditation";

    public const string CreateRegistration = "Attempting to create new registration";
    public const string UpdateRegistrationSiteAddress = "Attempting to update registration site address";
    public const string UpdateRegistrationTaskStatus = "Attempting to update registration task status";
    public const string MarkAsDulyMade = "MarkAsDulyMadeBy id :{Id}";
    public const string MarkAccreditationAsDulyMade = "MarkAccreditationAsDulyMadeBy id :{Id}";
    public const string RegistrationMaterialReference = "Attempting to get reference data registration material id :{Id}";
    public const string CreateRegistrationMaterialAndExemptionReferences = "Attempting to create new registration material and exemption references";
}