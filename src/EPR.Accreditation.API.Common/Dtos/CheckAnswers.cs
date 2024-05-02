namespace EPR.Accreditation.API.Common.Dtos
{
    public class CheckAnswers
    {
        public Guid Id { get; set; }

        public bool Completed { get; set; }

        public Address SiteAddress { get; set; }

        public List<CheckAnswersSectionDto> Sections { get; set; }
    }
}