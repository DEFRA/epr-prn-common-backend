namespace EPR.Accreditation.API.Common.Dtos
{
    public class SaveAndContinue
    {
        public Guid AccreditationId { get; set; }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Parameters { get; set; }
    }
}
