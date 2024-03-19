namespace EPR.Accreditation.API.Common.Dtos
{
    public abstract class ExternalIdBase
    {
        protected ExternalIdBase()
        {
            ExternalId = Guid.NewGuid();
        }

        public Guid? ExternalId { get; set; }
    }
}
