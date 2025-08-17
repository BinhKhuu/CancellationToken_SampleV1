using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web.Resource;

namespace CancellationToken.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
        
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowDebug",
                builder => builder.WithOrigins("https://localhost:4200")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();  // 🔐 Must be before UseAuthorization
        app.UseAuthorization();
        app.MapControllers();
        app.UseCors("AllowDebug");
        app.Run();
    }
}