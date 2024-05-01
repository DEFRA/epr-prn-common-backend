namespace EPR.Accreditation.API.Common.Dtos
{
    public class CheckAnswers
    {
        public string SiteAddress { get; set; }

        public ICollection<CheckAnswersSectionDto> Sections { get; set; }
    }
}