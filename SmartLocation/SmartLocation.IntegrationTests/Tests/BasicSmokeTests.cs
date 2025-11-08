using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SmartLocation.IntegrationTests;

public class BasicSmokeTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BasicSmokeTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Health_DeveResponderOk()
    {
        var resp = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task MlPredict_DeveResponderOk()
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "/api/v1/ml/predict");
        req.Headers.Add("X-API-KEY", "SEGREDO-DA-SUA-API");
        req.Content = new StringContent("{"km":7000,"falhas":1,"idadeMeses":10}", System.Text.Encoding.UTF8, "application/json");
        var resp = await _client.SendAsync(req);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }
}