namespace CancellationToken.Tests.ControllerTests;
using Microsoft.AspNetCore.Mvc.Testing;

public class CancellationTokenControllerTests: IClassFixture<WebApplicationFactory<CancellationToken.Web.Program>>
{
    private readonly HttpClient _client;
    public CancellationTokenControllerTests(WebApplicationFactory<Web.Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task LongRunning_Timout_ShouldThrowCancelException()
    {
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
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(6));
        var request = _client.GetAsync("/api/CancellationToken/LongRunning", cts.Token);
        var response = await request;
        response.EnsureSuccessStatusCode();
    }
}