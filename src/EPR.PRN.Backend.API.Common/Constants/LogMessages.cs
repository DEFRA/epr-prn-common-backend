namespace EPR.PRN.Backend.API.Common.Constants;

public static class LogMessages
{
    public const string RegistrationMaterialsTasks = "Attempting to get registration with materials and tasks";
    public const string AccreditationMaterialsTasks = "Attempting to get registration with materials, accreditations and tasks";
    public const string RegistrationAccreditationsMaterialsTasks = "Attempting to get registration accreditations with materials and tasks";
    public const string SummaryInfoMaterial = "Attempting to get summary info for a material";
    public const string OutcomeMaterialRegistration = "UpdateRegistrationOutcome called with Id: {Id}";
    public const string RegistrationSiteAddress = "Attempting to get site address info for registration id :{Id}";
    public const string MaterialAuthorization = "Attempting to get material authorisation  site of registration id :{Id}";
    public const string RegistrationMaterialpaymentFees = "Attempting to get registration material payment fee by id :{Id}";
    public const string UpdateRegulatorApplicationTask = "UpdateRegulatorApplicationTask";
    public const string UpdateRegulatorRegistrationTask = "UpdateRegulatorRegistrationTask";
    public const string UpdateRegulatorAccreditationTask = "UpdateRegulatorAccreditationTask";
    public const string AccreditationSamplingPlan = "Attempting to get file uploads relating to an accreditation";
    public const string CreateRegistrationMaterial = "Attempting to create new registration material for registration {0}";
    public const string GetAllRegistrationMaterials = "Attempting to retrieve all registration materials for registration {0}";
    public const string DeleteRegistrationMaterial = "Attempting to delete registration material with ID {0}";

    public const string CreateRegistration = "Attempting to create new registration";
    public const string UpdateRegistration = "Attempting to create new registration with ID {0}";
    public const string UpdateRegistrationMaterial = "Attempting to update registration material with External ID {Id}";
    public const string UpdateRegistrationMaterialPermits = "Attempting to update registration material permits with External ID {Id}";
    public const string UpdateRegistrationMaterialPermitCapacity = "Attempting to update registration material permit capacity with External ID {Id}";
    public const string GetRegistrationByOrganisation = "Attempting to get registration of type {0} for organisation with ID {1}";
    public const string UpdateRegistrationSiteAddress = "Attempting to update registration site address";
    public const string UpdateRegistrationTaskStatus = "Attempting to update registration task status";
    public const string MarkAsDulyMade = "MarkAsDulyMadeBy id :{Id}";
    public const string MarkAccreditationAsDulyMade = "MarkAccreditationAsDulyMadeBy id :{Id}";
    public const string RegistrationMaterialReference = "Attempting to get reference data registration material id :{Id}";
    public const string CreateExemptionReferences = "Attempting to create exemption references";
    public const string GetMaterialsPermitTypes = "Attempting to get material permit types";
}