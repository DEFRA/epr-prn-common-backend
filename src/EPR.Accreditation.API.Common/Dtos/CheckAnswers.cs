namespace EPR.Accreditation.API.Common.Dtos
{
    public class CheckAnswers
    {
        public Address SiteAddress { get; set; }

        public List<CheckAnswersSectionDto> Sections { get; set; }
    }
}