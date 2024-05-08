namespace EPR.Accreditation.API.Common.Dtos
{
    public class CheckAnswersSectionRow
    {
        /// <summary>
        /// Gets or sets the name of the row
        /// </summary>
        public string TitleKey { get; set; }

        /// <summary>
        /// Gets or sets the answer for a row
        /// </summary>
        public List<string> Value { get; set; }

        /// <summary>
        /// Gets or sets the URL for the change link
        /// </summary>
        public string ChangeLink { get; set; }
    }
}