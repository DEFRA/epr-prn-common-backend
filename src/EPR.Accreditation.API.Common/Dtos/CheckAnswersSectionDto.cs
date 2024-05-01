namespace EPR.Accreditation.API.Common.Dtos
{
    using EPR.Accreditation.Facade.Common.Dtos;

    public class CheckAnswersSectionDto
    {
        public Guid Id { get; set; }

        public bool Completed { get; set; }

        public List<CheckAnswersSectionRow> SectionRows { get; set; } = new();

        public Dictionary<string, string> QueryStringRouteData { get; set; } = new();
    }
}