using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace CancellationToken.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<CancellationToken.Web.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            config
                .AddJsonFile((
                        Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")),
                    optional: false,
                    reloadOnChange: true)
                .AddEnvironmentVariables();
        });
    }
}