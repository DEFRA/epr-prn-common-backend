namespace EPR.Accreditation.API.Common.Dtos
{
    public class CheckAnswersSectionDto
    {
        /// <summary>
        /// Gets or sets the title of the section
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets whether the section has been completed or not
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Gets or sets the list of rows for the section
        /// </summary>
        public List<CheckAnswersSectionRow> SectionRows { get; set; }
    }
}