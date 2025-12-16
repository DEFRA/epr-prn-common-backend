using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

public static class TestUtilsExtensions
{
    private static void ShouldHaveStatus(this HttpResponseMessage response, HttpStatusCode status)
    {
        response.StatusCode.Should().Be(status);
    }

    public static async Task<string> CallGetEndpoint(
        this CustomWebApplicationFactory<Startup> application,
        string url,
        HttpStatusCode expectedResponse
    )
    {
        application.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "TestScheme",
            "test-token"
        );
        var response = await application.Client.GetAsync(url);

        response.ShouldHaveStatus(expectedResponse);
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<T> CallGetEndpoint<T>(
        this CustomWebApplicationFactory<Startup> application,
        string url,
        HttpStatusCode expectedResponse = HttpStatusCode.OK,
        NameValueCollection queryParameters = null
    )
    {
        var uriBuilder = new UriBuilder(new Uri(application.Client.BaseAddress!, url));

        if (queryParameters is not null)
            uriBuilder.Query = queryParameters.ToString() ?? "";

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = uriBuilder.Uri,
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("TestScheme", "test-token");

        var response = await application.Client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }

        response.ShouldHaveStatus(expectedResponse);
        return await response.GetContent<T>();
    }

    public static async Task<TResponse> CallGetEndpoint<TResponse, TQuery>(
        this CustomWebApplicationFactory<Startup> application,
        string url,
        TQuery query,
        HttpStatusCode expectedResponse = HttpStatusCode.OK
    )
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            Headers = { Authorization = new AuthenticationHeaderValue("TestScheme", "test-token") },
            RequestUri = new Uri(application.Client.BaseAddress!, url + ToQueryString(query)),
        };
        var response = await application.Client.SendAsync(request);
        response.ShouldHaveStatus(expectedResponse);
        return await response.GetContent<TResponse>();
    }

    private static string ToQueryString<T>(T obj)
    {
        if (obj == null)
            return string.Empty;

        var properties = typeof(T)
            .GetProperties()
            .Where(p => p.GetValue(obj) != null)
            .Select(p =>
                $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.GetValue(obj)!.ToString()!)}"
            );

        return "?" + string.Join("&", properties);
    }

    private static async Task<T> GetContent<T>(this HttpResponseMessage response)
    {
        var dataStr = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<T>(dataStr);
        data.Should().NotBeNull();
        return data!;
    }

    public static async Task CallDeleteEndpoint(
        this CustomWebApplicationFactory<Startup> application,
        string url,
        HttpStatusCode expectedResponse = HttpStatusCode.NoContent
    )
    {
        application.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "TestScheme",
            "test-token"
        );
        var response = await application.Client.DeleteAsync(url);
        response.StatusCode.Should().Be(expectedResponse);
    }

    public static async Task CallDeleteEndpoint<TRequest>(
        this CustomWebApplicationFactory<Startup> application,
        string url,
        TRequest request,
        HttpStatusCode expectedResponse = HttpStatusCode.NoContent
    )
    {
        var response = await application.Client.SendAsync(
            new HttpRequestMessage
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(request),
                    new MediaTypeHeaderValue(HttpContentTypes.Json)
                ),
                Method = HttpMethod.Delete,
                Headers =
                {
                    Authorization = new AuthenticationHeaderValue("TestScheme", "test-token"),
                },
                RequestUri = new Uri(application.Client.BaseAddress!, url),
            }
        );
        response.StatusCode.Should().Be(expectedResponse);
    }

    public static async Task<(T cloned, string location)> CallCloneEndpoint<T>(
        this CustomWebApplicationFactory<Startup> application,
        string url,
        HttpStatusCode expectedStatus = HttpStatusCode.Created
    )
        where T : class
    {
        application.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            "test-token"
        );
        var response = await application.Client.PostAsync(url, null);
        response.ShouldHaveStatus(expectedStatus);
        return (await response.GetContent<T>(), response.Headers.Location?.ToString() ?? "");
    }

    public static async Task<TResponse> CallPutEndpoint<TRequest, TResponse>(
        this CustomWebApplicationFactory<Startup> application,
        string url,
        TRequest request,
        HttpStatusCode expectedStatus = HttpStatusCode.OK
    )
        where TResponse : class
        where TRequest : class
    {
        application.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            "test-token"
        );
        var response = await application.Client.PutAsync(
            url,
            new StringContent(
                JsonConvert.SerializeObject(request),
                new MediaTypeHeaderValue(HttpContentTypes.Json)
            )
        );
        var ret = await response.GetContent<TResponse>();
        response.ShouldHaveStatus(expectedStatus);
        return ret;
    }

    public static async Task CallPutEndpoint<TRequest>(
        this CustomWebApplicationFactory<Startup> application,
        string url,
        TRequest request,
        HttpStatusCode expectedStatus = HttpStatusCode.OK
    )
        where TRequest : class
    {
        application.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            "test-token"
        );
        var response = await application.Client.PutAsync(
            url,
            new StringContent(
                JsonConvert.SerializeObject(request),
                new MediaTypeHeaderValue(HttpContentTypes.Json)
            )
        );
        response.ShouldHaveStatus(expectedStatus);
    }

    public static async Task<(TCreated created, string location)> CallPostEndpoint<
        TRequest,
        TCreated
    >(
        this CustomWebApplicationFactory<Startup> application,
        string url,
        TRequest data,
        HttpStatusCode expectedStatus = HttpStatusCode.Created
    )
        where TRequest : class
        where TCreated : class
    {
        var response = await application.Client.PostAsync(
            url,
            new StringContent(
                JsonConvert.SerializeObject(data),
                new MediaTypeHeaderValue(HttpContentTypes.Json)
            )
        );
        if (response.StatusCode != expectedStatus)
        {
            var resp = await response.Content.ReadAsStringAsync();
            Console.WriteLine(resp);
        }

        response.ShouldHaveStatus(expectedStatus);

        return (await response.GetContent<TCreated>(), response.Headers.Location?.ToString() ?? "");
    }

    public static async Task<HttpResponseMessage> CallPostEndpoint<TRequest>(
        this CustomWebApplicationFactory<Startup> application,
        string url,
        TRequest data,
        HttpStatusCode expectedStatus = HttpStatusCode.Created
    )
        where TRequest : class
    {
       
        var response = await application.Client.PostAsync(
            url,
            new StringContent(
                JsonConvert.SerializeObject(data),
                new MediaTypeHeaderValue(HttpContentTypes.Json)
            )
        );
        response.ShouldHaveStatus(expectedStatus);
        
        return response;
    }

    public static async Task<HttpResponseMessage> CallPostEndpointWithJson(
        this CustomWebApplicationFactory<Startup> application,
        string url,
        string data,
        HttpStatusCode expectedStatus = HttpStatusCode.Created
    )
    {
        var response = await application.Client.PostAsync(
            url,
            new StringContent(
                data,
                new MediaTypeHeaderValue(HttpContentTypes.Json)
            )
        );
        response.ShouldHaveStatus(expectedStatus);
        
        return response;
    }
    public static async Task CallPostEndpoint(
        this CustomWebApplicationFactory<Startup> application,
        string url,
        HttpStatusCode expectedStatus = HttpStatusCode.Created
    )
    {
        application.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            "test-token"
        );
        var response = await application.Client.PostAsync(
            url,
            new StringContent("")
        );
        response.ShouldHaveStatus(expectedStatus);
    }

    public static void ReplaceService<T>(this IServiceCollection services, T service)
        where T : class
    {
        var descriptors = services
            .Where(d => d.ServiceType == typeof(T))
            .ToList();

        // Remove each matching service
        foreach (var descriptor in descriptors)
            services.Remove(descriptor);
        services.AddSingleton(service);
    }
    public static async Task ShouldHaveRequiredErrorMessage(this HttpResponseMessage response, string propertyName)
    {
        response.IsSuccessStatusCode.Should().BeFalse();
        var content = await response.Content.ReadAsStringAsync();
        string.IsNullOrWhiteSpace(content).Should().BeFalse();
        var jObj = JObject.Parse(content);
        jObj["title"]!.Value<string>().Should().Be("One or more validation errors occurred.");
        var errs = jObj["errors"]?["$"]?.Select(s => s.Value<string>())!.ToList();
        Console.WriteLine(string.Join("\n", errs));
        errs!.FirstOrDefault(s => s.Contains(propertyName, StringComparison.CurrentCultureIgnoreCase)).Should().NotBeNull();
    }
    public static async Task ShouldHaveValidationErrorMessage(this HttpResponseMessage response, string propertyName)
    {
        response.IsSuccessStatusCode.Should().BeFalse();
        var content = await response.Content.ReadAsStringAsync();
        string.IsNullOrWhiteSpace(content).Should().BeFalse();
        Console.WriteLine(content);
        var jObj = JObject.Parse(content);
        jObj["title"]!.Value<string>().Should().Be("One or more validation errors occurred.");
        jObj["errors"]?[propertyName].Should().NotBeNull();
    }
}

public static class HttpContentTypes
{
    public const string Json = "application/json";
}