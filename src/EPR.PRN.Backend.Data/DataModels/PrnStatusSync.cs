namespace EPR.PRN.Backend.Data.DataModels
{
    public class PrnStatusSync
    {
        public string StatusName { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; }
        public string OrganisationName { get; set; } = string.Empty;
        public string PrnNumber { get; set; } = string.Empty;
    }
}
