using System.Security.Claims;
using CancellationToken.Tests.Helpers;

namespace CancellationToken.Tests.ControllerTests;
using Microsoft.AspNetCore.Mvc.Testing;

public class CancellationTokenControllerTests: IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;
    public CancellationTokenControllerTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();

    }

    [Fact]
    public async Task LongRunning_Timout_ShouldThrowCancelException()
    {
        var token = MockJwtTokens.GenerateJwtToken([new Claim("email","test@email.com")]);
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var request = _client.GetAsync("/api/CancellationToken/LongRunning", cts.Token);
        
        await Assert.ThrowsAsync<TaskCanceledException>(() => request);

    }

    [Fact]
    public async Task LongRunning_Cancelled_ShouldThrowCancelException()
    {
        using var cts = new CancellationTokenSource();
        var request = _client.GetAsync("/api/CancellationToken/LongRunning", cts.Token);
        cts.Cancel();
        await Assert.ThrowsAsync<TaskCanceledException>(() => request);
    }

    [Fact]
    public async Task LongRunning_InTime_ShouldReturnSucess()
    {
        var token = MockJwtTokens.GenerateJwtToken([new Claim("email","test@email.com")]);
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        using var cts = new CancellationTokenSource();
        
        var request = _client.GetAsync("/api/CancellationToken/LongRunning", cts.Token);
        var response = await request;
        response.EnsureSuccessStatusCode();
    }

    private string GetToken()
    {
        return MockJwtTokens.GenerateJwtToken([
        ]);
    }
}