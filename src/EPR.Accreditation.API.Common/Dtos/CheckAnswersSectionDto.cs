namespace EPR.Accreditation.API.Common.Dtos
{
    public class CheckAnswersSectionDto
    {
        public string Title { get; set; }

        public bool Completed { get; set; }

        public List<CheckAnswersSectionRow> SectionRows { get; set; }
    }
}