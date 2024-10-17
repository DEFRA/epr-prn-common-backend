namespace EPR.PRN.Backend.Obligation.Constants
{
    public static class ObligationConstants
    {
        public static class Statuses
        {
            public const string NoDataYet = "NoDataYet";
            public const string Met = "Met";
            public const string NotMet = "NotMet";
        }

        public static class ErrorMessages
        {
            public const string NoMaterialsFoundErrorMessage = "No Materials found in PRN BAckend Database";
            public const string NoObligationDataForOrganisationIdErrorMessage = "Obligation calculation not found for Organisation Id";
        }
    }
}
