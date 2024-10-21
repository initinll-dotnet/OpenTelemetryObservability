using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using System.Reflection;

namespace WebApi.Diagnostics;

public static class OpenTelemetryConfigurationExtensions
{
    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        const string serviceName = "WebApi";

        var version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();

        builder.Services
        .AddOpenTelemetry()
        .ConfigureResource(resource =>
        {
            resource
                .AddService(
                    serviceName: serviceName,
                    serviceNamespace: "Learning OpenTelemetry",
                    serviceVersion: version)
                .AddAttributes(
                [
                    new KeyValuePair<string, object>("service.environment", "development") // custom attribute
                ]);
        })
        .WithTracing(tracing =>
        {
            tracing
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter()
                .AddOtlpExporter(otlpOptions =>
                {
                    // https://www.jaegertracing.io/docs/1.62/getting-started/
                    otlpOptions.Endpoint = new Uri("http://jaeger:4317"); // Jaeger endpoint via docker compose
                });
        });

        return builder;
    }
}
