using GrpcService;

using System.Diagnostics;

using WebApi.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

var greeterGrpcUrl = builder.Configuration["GrpcSettings:GreeterUrl"]!;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();

builder.Services.AddGrpcClient<Greeter.GreeterClient>(options =>
{
    options.Address = new Uri(greeterGrpcUrl);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});

builder.AddOpenTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// This will add the Prometheus scraping endpoint to the pipeline so that Prometheus can scrape metrics from this app
// Prometheus uses pull-based monitoring, so it will scrape metrics from this endpoint at a regular interval based on prometheus configuration in prometheus.yml
// nuget package: OpenTelemetry.Exporter.Prometheus
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    // Custom meter added in ApplicationDiagnostics.cs
    ApplicationDiagnostics
    .WeatherForecastCounter
    .Add(delta: 1, 
        tags: [new KeyValuePair<string, object?>(key: "client.api", value: "weatherforecast")]);

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/sayhello/{name}", (string name, Greeter.GreeterClient greeterClient) =>
{
    // additional context - custom tag
    Activity.Current?.SetGreeterName(name);

    var helloResponse = greeterClient.SayHello(new HelloRequest { Name = name });
    var msg = helloResponse.Message;
    return msg;
})
.WithName("Greeter")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
