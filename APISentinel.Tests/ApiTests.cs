using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace APISentinel.Tests;

public sealed class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public ApiTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact]
    public async Task Health_endpoint_is_available()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Resource_can_be_created_and_retrieved()
    {
        var response = await _client.PostAsJsonAsync("/api/endpointchecks", new
        {
            url = "https://example.com/health",
            expectedStatus = 200,
            isEnabled = true
        });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var location = response.Headers.Location;
        Assert.NotNull(location);
        var read = await _client.GetAsync(location);
        Assert.Equal(HttpStatusCode.OK, read.StatusCode);
    }

    [Theory]
    [InlineData("example.com", 200)]
    [InlineData("https://example.com/health", 99)]
    [InlineData("https://example.com/health", 600)]
    public async Task Invalid_resource_data_is_rejected(string url, int expectedStatus)
    {
        var response = await _client.PostAsJsonAsync("/api/endpointchecks", new
        {
            url,
            expectedStatus,
            isEnabled = true
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Missing_resource_returns_not_found()
    {
        var response = await _client.GetAsync("/api/endpointchecks/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
