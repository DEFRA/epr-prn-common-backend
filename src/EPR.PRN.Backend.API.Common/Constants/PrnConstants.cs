namespace EPR.PRN.Backend.API.Common.Constants
{
    public static class PrnConstants
    {
        // Max length constants
        public const int MaxLengthPrnNumber = 20;
        public const int MaxLengthOrganisationName = 160;
        public const int MaxLengthProducerAgency = 50;
        public const int MaxLengthReprocessorExporterAgency = 50;
        public const int MaxLengthMaterialName = 20;
        public const int MaxLengthIssuerNotes = 200;
        public const int MaxLengthIssuerReference = 200;
        public const int MaxLengthPrnSignatory = 50;
        public const int MaxLengthPrnSignatoryPosition = 50;
        public const int MaxLengthSignature = 100;
        public const int MaxLengthProcessToBeUsed = 20;
        public const int MaxLengthIssuedByOrg = 50;
        public const int MaxLengthAccreditationNumber = 20;
        public const int MaxLengthReprocessingSite = 100;
        public const int MaxLengthAccreditationYear = 10;
        public const int MaxLengthObligationYear = 10;
        public const int MaxLengthPackagingProducer = 100;
        public const int MaxLengthCreatedBy = 20;
        public const int MaxLengthSourceSystemId = 40;
        public const int ObligationYearDefault = 9999;

        public static class Filters
        {
            public const string AcceptedAll = "accepted-all";
            public const string CancelledAll = "cancelled-all";
            public const string RejectedAll = "rejected-all";
            public const string AwaitingAll = "awaiting-all";
            public const string AwaitingAluminium = "awaiting-aluminium";
            public const string AwaitingGlassOther = "awaiting-glassother";
            public const string AwaitingGlassMelt = "awaiting-glassremelt";
            public const string AwaitngPaperFiber = "awaiting-paperfiber";
            public const string AwaitngPlastic = "awaiting-plastic";
            public const string AwaitngSteel = "awaiting-steel";
            public const string AwaitngWood = "awaiting-wood";
        }

        public static class Sorts
        {
            public const string Descending = "desc";
            public const string Ascending = "asc";
            public const string IssueDateDesc = "date-issued-desc";
            public const string IssueDateAsc = "date-issued-asc";
            public const string TonnageDesc = "tonnage-desc";
            public const string TonnageAsc = "tonnage-asc";
            public const string IssuedByDesc = "issued-by-desc";
            public const string IssuedByAsc = "issued-by-asc";
            public const string DescemberWasteDesc = "december-waste-desc";
            public const string MaterialDesc = "material-desc";
            public const string MaterialAsc = "material-asc";
        }

        public static class Materials
        {
            public const string Aluminium = "Aluminium";
            public const string GlassOther = "Glass Other";
            public const string GlassMelt = "Glass Re-melt";
            public const string PaperFiber = "Paper/board";
            public const string Plastic = "Plastic";
            public const string Steel = "Steel";
            public const string Wood = "Wood";
            public const string PaperComposting = "Paper Composting";
            public const string WoodComposting = "Wood Composting";
        }

        public static class ExporterCodePrefixes
        {
            public const string EaExport = "EX";
            public const string SepaExport = "SX";
        }
    }
}
