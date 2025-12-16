using EPR.PRN.Backend.API.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public CustomWebApplicationFactory()
    {
        Client = CreateClient();
    }

    public HttpClient Client { get; }

    public Mock<IPrnService> PrnService { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("UnitTest");
        builder.ConfigureTestServices(services => { services.ReplaceService(PrnService.Object); });
    }
}