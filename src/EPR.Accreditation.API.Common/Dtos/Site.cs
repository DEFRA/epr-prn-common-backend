namespace EPR.Accreditation.API.Common.Dtos
{
    public class Site
    {
        public Address Address { get; set; }

        public IEnumerable<SiteAuthority> SiteAuthorities { get; set; }

        public IEnumerable<string> ExemptionReferences { get; set; }
    }
}
