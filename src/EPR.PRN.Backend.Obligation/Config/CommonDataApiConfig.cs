namespace EPR.PRN.Backend.Obligation.Config
{
    public class CommonDataApiConfig
    {
        public const string SectionName = "CommonDataApiConfig";

        public string BaseUrl { get; set; } = null!;

        public int Timeout { get; set; }

        public int ServiceRetryCount { get; set; }

        public string GetAggregatedPomData { get; set; } = null!;

    }
}
