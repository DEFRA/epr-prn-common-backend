namespace EPR.PRN.Backend.API.Constants;

public static class ValidationMessages
{
    public const string RegistrationOutcomeIdRequired = "Id is required.";
    public const string RegistrationOutcomeIdGreaterThanZero = "Id must be greater than zero.";
    public const string InvalidRegistrationOutcomeStatus = "Invalid registration material status.";
    public const string RegistrationOutcomeCommentsMaxLength = "RegistrationMaterial Comment cannot exceed 500 characters.";
    public const string RegistrationOutcomeCommentsCommentsRequired = "Comments are required.";
}
