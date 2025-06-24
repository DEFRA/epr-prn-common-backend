namespace EPR.PRN.Backend.API.Constants;

public static class ValidationMessages
{
    public const string RegistrationOutcomeIdRequired = "Id is required.";
    public const string InvalidRegistrationOutcomeStatus = "Invalid registration material status.";
    public const string RegistrationOutcomeCommentsMaxLength = "RegistrationMaterial Comment cannot exceed 500 characters.";
    public const string RegistrationOutcomeCommentsRequired = "Comments are required.";
    public const string RegistrationOutcomeUserRequired = "User is required.";

    public const string RegistrationMaterialIdRequired = "Id is required.";
    public const string RegistrationAccreditationIdRequired = "Accreditation Id is required.";
    public const string InvalidDulyMadeDate = "Invalid registration material DulyMadeDate is invalid.";
    public const string InvalidDeterminationDate = "Invalid registration material DeterminationDate is invalid.";
    public const string DeterminationDate12WeekRule = "DeterminationDate must be at least 12 weeks after DulyMadeDate.";
    public const string DulyMadeByRequired= "DulyMadeBy is required.";
    public const string RegistrationOrganisationIdRequired = "Orgnaisation Id is required.";   
}
