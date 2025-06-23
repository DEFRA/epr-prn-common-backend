namespace EPR.PRN.Backend.API.Dto.Regulator
{
    public class AccreditationBusinessPlanDto : NoteBase
    {
        public Guid AccreditationId { get; set; }
        public required string SiteAddress { get; set; }
        public required string MaterialName { get; set; }
        public decimal InfrastructurePercentage { get; set; }
        public required string InfrastructureNotes { get; set; }
        public decimal RecycledWastePercentage { get; set; }
        public required string RecycledWasteNotes { get; set; }
        public decimal BusinessCollectionsPercentage { get; set; }
        public required string BusinessCollectionsNotes { get; set; }
        public decimal CommunicationsPercentage { get; set; }
        public required string CommunicationsNotes { get; set; }
        public decimal NewMarketsPercentage { get; set; }
        public required string NewMarketsNotes { get; set; }
        public decimal NewUsersRecycledPackagingWastePercentage { get; set; }
        public required string NewUsersRecycledPackagingWasteNotes { get; set; }
        public decimal NotCoveredOtherCategoriesPercentage { get; set; }
        public required string NotCoveredOtherCategoriesNotes { get; set; }
    }
}