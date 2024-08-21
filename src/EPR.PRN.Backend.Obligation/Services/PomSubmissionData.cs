using EPR.PRN.Backend.Obligation.Config;
using EPR.PRN.Backend.Obligation.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EPR.PRN.Backend.Obligation.Services
{
    public class PomSubmissionData : IPomSubmissionData
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PomSubmissionData> _logger;
        private readonly CommonDataApiConfig _config;

        public PomSubmissionData(HttpClient httpClient, ILogger<PomSubmissionData> logger, IOptions<CommonDataApiConfig> options)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = options.Value;
        }

        public async Task<HttpResponseMessage> GetAggregatedPomData(string submissionIdString)
        {
            var url = string.Format($"{_config.GetAggregatedPomData}", submissionIdString);

            _logger.LogInformation("Attempting to fetch Aggregated POM data by Submission Id '{submissionIdString}'", submissionIdString);

            return await _httpClient.GetAsync(url);
        }
    }
}
