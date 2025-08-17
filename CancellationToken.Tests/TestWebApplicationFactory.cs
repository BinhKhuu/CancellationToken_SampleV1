using CancellationToken.Tests.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

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

        builder.ConfigureServices(PostConfigureBearer);
    }

    private void ConfigureTestServices(IServiceCollection services)
    {
        //  configuring bearer can be done a few ways, only run one way
        PostConfigureBearer(services); // override configuration from the web apps program.cs - must make sure you target all parameters
        //RemoveAndAddBearer(services); // remove target services and add your own
    }

    /// <summary>
    /// Remove target services and then add your own configuration
    /// </summary>
    /// <param name="services"></param>
    private void RemoveAndAddBearer(IServiceCollection services)
    {
        var servicesToRemove = new List<Type>
        {
            typeof(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerOptions),
        };
        
        // Remove any authentication-related services that might interfere
        var authServices = services.Where(d => 
            d.ServiceType.FullName?.Contains("Authentication") == true ||
            d.ImplementationType?.FullName?.Contains("Authentication") == true).ToList();
        
        foreach (var service in authServices)
        {
            services.Remove(service);
        }
        services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var config = new OpenIdConnectConfiguration()
            {
                Issuer = MockJwtTokens.Issuer
            };
        
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidAudience = "your-api-audience"
            };
        
            config.SigningKeys.Add(MockJwtTokens.SecurityKey);
            options.Configuration = config;
        });
            
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Auth failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token validated");
                        return Task.CompletedTask;
                    }
                };
            });        
    }

    /// <summary>
    /// Use Post Configure to override the bearer settings
    /// </summary>
    /// <param name="services"></param>
    private void PostConfigureBearer(IServiceCollection services)
    {
        services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var config = new OpenIdConnectConfiguration()
            {
                Issuer = MockJwtTokens.Issuer
            };

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidAudience = "your-api-audience",
                ValidIssuer = MockJwtTokens.Issuer,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = MockJwtTokens.SecurityKey
            };

            config.SigningKeys.Add(MockJwtTokens.SecurityKey);
            options.Configuration = config;
            
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Auth failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("Token validated");
                    return Task.CompletedTask;
                }
            };
        });
    }
}