namespace EPR.Accreditation.Facade.Common.Dtos
{
    public class CheckAnswersSectionRow
    {
        public string ListKeyEn { get; set; }

        public string ListValueEn { get; set; }

        public string ListKeyCy { get; set; }

        public string ListValueCy { get; set; }

        public string Controller { get; set; }

        public string ControllerAction { get; set; }

        public string Area { get; set; }

        public IDictionary<string, string> RouteData { get; set; }

        public IDictionary<string, string> RouteAction { get; set; }
    }
}