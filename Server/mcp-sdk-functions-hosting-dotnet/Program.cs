using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.AspNetCore;
using QuickstartWeatherServer.Tools;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("FUNCTIONS_CUSTOMHANDLER_PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddMcpServer()
    .WithHttpTransport((options) =>
    {
        options.Stateless = true;
    })
    .WithTools<WeatherTools>()
    .WithTools<PizzaTools>();

builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services.AddSingleton(_ =>
{
    var client = new HttpClient { BaseAddress = new Uri("https://api.weather.gov") };
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("weather-tool", "1.0"));
    return client;
});

var app = builder.Build();

// Add health check endpoint
app.MapGet("/api/healthz", () => "Healthy");

// Map MCP endpoints
app.MapMcp(pattern: "/mcp");

await app.RunAsync();
