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
    public const string CreateRegistrationMaterial = "Attempting to create new registration material for registration {RegistrationId}";
    public const string GetAllRegistrationMaterials = "Attempting to retrieve all registration materials for registration {RegistrationId}";
    public const string DeleteRegistrationMaterial = "Attempting to delete registration material with ID {RegistrationMaterialId}";
    public const string UpsertRegistrationMaterialContact = "Attempting to upsert the contact for registration material with ID {Id}";
    public const string AccreditationBusinessPlan = "Attempting to get Business Plan relating to an accreditation";
    public const string SaveOverseasReprocessingSites = "Attempting to save overseas reprocessing sites with ID :{registrationMaterialId}";
    public const string UpdateMaximumWeight = "Attempting to update the maximum weight the site is capable of processing for the material {RegistrationMaterialId}.";
    public const string GetOverseasMaterialReprocessingSites = "Attempting to retrieve overseas reprocessing sites including corresponding interim sites for registrationMaterial {RegistrationMaterialId}";

    public const string CreateRegistration = "Attempting to create new registration";
    public const string UpdateRegistration = "Attempting to create new registration with ID {0}";
    public const string UpdateRegistrationMaterial = "Attempting to update registration material with External ID {Id}";
    public const string UpdateRegistrationMaterialPermits = "Attempting to update registration material permits with External ID {Id}";
    public const string UpdateRegistrationMaterialPermitCapacity = "Attempting to update registration material permit capacity with External ID {Id}";
    public const string GetRegistrationByOrganisation = "Attempting to get registration of type {0} for organisation with ID {1}";
    public const string UpdateRegistrationSiteAddress = "Attempting to update registration site address";
    public const string UpdateRegistrationTaskStatus = "Attempting to update registration task status";
    public const string UpdateApplicationRegistrationTaskStatus = "Attempting to update application registration task status";
    public const string MarkAsDulyMade = "MarkAsDulyMadeBy id :{Id}";
    public const string MarkAccreditationAsDulyMade = "MarkAccreditationAsDulyMadeBy id :{Id}";
    public const string RegistrationMaterialReference = "Attempting to get reference data registration material id :{Id}";
    public const string CreateExemptionReferences = "Attempting to create exemption references";
    public const string GetMaterialsPermitTypes = "Attempting to get material permit types";
    public const string CreateRegistrationMaterialAndExemptionReferences = "Attempting to create new registration material and exemption references";
    public const string RegistrationsOverview = "Attempting to retrieve registrations overview for organisation id :{Id}";    
	public const string UpdateIsMaterialRegistered = "Attempting to update the registration material IsMaterialRegistered flag.";
    public const string UpsertRegistrationReprocessingDetails = "Attempting to upsert the registration reprocessing details for registration material with ID :{registrationMaterialId}";
    public const string UpdateMaterialNotReprocessingReason = "Attempting to update the reason for not reprocessing registration material with ID {RegistrationMaterialId}";
}