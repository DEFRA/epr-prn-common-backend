using EPR.Accreditation.Facade.Common.Dtos;

namespace EPR.Accreditation.API.Common.Dtos
{
    public class CheckAnswers
    {
        public Guid Id { get; set; }

        public bool Completed { get; set; }

        public string SiteAddress { get; set; }

        public List<CheckAnswersRowDto> SectionRows { get; set; } = new();

        public Dictionary<string, string> QueryStringRouteData { get; set; } = new();
    }
}
