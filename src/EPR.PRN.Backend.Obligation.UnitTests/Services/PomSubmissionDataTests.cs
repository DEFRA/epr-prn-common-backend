using System.Net;
using EPR.PRN.Backend.Obligation.Config;
using EPR.PRN.Backend.Obligation.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace EPR.PRN.Backend.Obligation.UnitTests.Services
{
    [TestClass]
    public class PomSubmissionDataTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private Mock<ILogger<PomSubmissionData>> _loggerMock;
        private Mock<IOptions<CommonDataApiConfig>> _optionsMock;
        private HttpClient _httpClient;
        private PomSubmissionData _service;
        private CommonDataApiConfig _config;

        [TestInitialize]
        public void SetUp()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _loggerMock = new Mock<ILogger<PomSubmissionData>>();
            _optionsMock = new Mock<IOptions<CommonDataApiConfig>>();

            _config = new CommonDataApiConfig { GetAggregatedPomData = "https://api.example.com/pomdata/{0}" };
            _optionsMock.Setup(x => x.Value).Returns(_config);

            _service = new PomSubmissionData(_httpClient, _loggerMock.Object, _optionsMock.Object);
        }

        [TestMethod]
        public async Task GetAggregatedPomData_ReturnsHttpResponseMessage()
        {
            // Arrange
            var submissionIdString = "123";
            var expectedUrl = string.Format(_config.GetAggregatedPomData, submissionIdString);
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == expectedUrl),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.GetAggregatedPomData(submissionIdString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == expectedUrl),
                ItExpr.IsAny<CancellationToken>());

            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Attempting to fetch Aggregated POM data by Submission Id '{submissionIdString}'")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task GetAggregatedPomData_LogsError_WhenRequestFails()
        {
            // Arrange
            var submissionIdString = "123";
            var expectedUrl = string.Format(_config.GetAggregatedPomData, submissionIdString);
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == expectedUrl),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.GetAggregatedPomData(submissionIdString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Attempting to fetch Aggregated POM data by Submission Id '{submissionIdString}'")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
