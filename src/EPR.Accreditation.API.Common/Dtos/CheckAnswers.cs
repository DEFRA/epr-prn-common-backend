namespace EPR.Accreditation.API.Common.Dtos
{
    public class CheckAnswers
    {
        /// <summary>
        /// Gets or sets the accreditation ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets whether all sections have been completed for the page or not
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Gets or sets the address of the site
        /// </summary>
        public Address SiteAddress { get; set; }

        /// <summary>
        /// Gets or sets the list of sections for the page
        /// </summary>
        public List<CheckAnswersSectionDto> Sections { get; set; }
    }
}