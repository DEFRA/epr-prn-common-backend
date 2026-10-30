using System.Net;
using AwesomeAssertions;
using BackendAccountService.Core.Models.Request;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Configs;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Obligation.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

/// <summary>
/// The search endpoint over HTTP, on a host that runs only MVC and the real PrnController (its dependencies mocked),
/// so the framework's model validation is exercised without the application's database.
/// </summary>
[TestClass]
public class PrnSearchRequestTests
{
    private sealed class SearchHost : IAsyncDisposable
    {
        private readonly WebApplication _app;

        private SearchHost(WebApplication app, Mock<IPrnService> prnService)
        {
            _app = app;
            PrnService = prnService;
            Client = app.GetTestClient();
        }

        public Mock<IPrnService> PrnService { get; }

        public HttpClient Client { get; }

        public static async Task<SearchHost> StartAsync()
        {
            var prnService = new Mock<IPrnService>();
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseTestServer();
            builder.Services.AddControllers().AddApplicationPart(typeof(PrnController).Assembly);
            builder.Services.AddApiVersioning();
            builder.Services.AddSingleton(prnService.Object);
            builder.Services.AddSingleton(new Mock<IObligationCalculatorService>().Object);
            builder.Services.AddSingleton(new Mock<IValidator<SaveNpwdPrnDetailsRequest>>().Object);
            builder.Services.AddSingleton(Options.Create(new PrnObligationCalculationConfig()));

            var app = builder.Build();
            app.MapControllers();
            await app.StartAsync();

            return new SearchHost(app, prnService);
        }

        public async ValueTask DisposeAsync()
        {
            Client.Dispose();
            await _app.DisposeAsync();
        }
    }

    private static HttpRequestMessage Search(string query) =>
        new(HttpMethod.Get, $"api/v1/prn/search{query}")
        {
            Headers = { { "X-EPR-ORGANISATION", Guid.NewGuid().ToString() } },
        };

    [TestMethod]
    [DataRow("0")]
    [DataRow("-1")]
    [DataRow("2022")]
    [DataRow("2051")]
    public async Task WhenAcceptableIntoYearIsOutOfRange_IsBadRequestAndNothingIsSearched(string year)
    {
        await using var host = await SearchHost.StartAsync();

        var response = await host.Client.SendAsync(Search($"?acceptableIntoYear={year}"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        host.PrnService.Verify(
            s => s.GetSearchPrnsForOrganisation(It.IsAny<Guid>(), It.IsAny<PaginatedRequestDto>()),
            Times.Never
        );
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("?acceptableIntoYear=2026")]
    [DataRow("?acceptableIntoYear=2023")]
    [DataRow("?acceptableIntoYear=2050")]
    public async Task WhenAcceptableIntoYearIsAbsentOrValid_Searches(string query)
    {
        await using var host = await SearchHost.StartAsync();
        host.PrnService
            .Setup(s => s.GetSearchPrnsForOrganisation(It.IsAny<Guid>(), It.IsAny<PaginatedRequestDto>()))
            .ReturnsAsync(new PaginatedResponseDto<PrnDto> { Items = [], TotalItems = 0 });

        var response = await host.Client.SendAsync(Search(query));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        host.PrnService.Verify(
            s => s.GetSearchPrnsForOrganisation(It.IsAny<Guid>(), It.IsAny<PaginatedRequestDto>()),
            Times.Once
        );
    }
}
