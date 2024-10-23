using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using System.Reflection;

namespace WebApi.Diagnostics;

public static class OpenTelemetryConfigurationExtensions
{
    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        const string serviceName = "WebApi";

        const string jaegerEndpoint = "http://jaeger:4317"; // Jaeger endpoint via docker compose
        const string aspireEndpoint = "http://aspire:18889"; // Aspire endpoint via docker compose
        const string otlpCollectorEndpoint = "http://opentelemetry-collector:4317"; // Collector endpoint via docker compose

        // note - by activating collector we are disabling direct backend communication by commenting director exporters in tracing and metrics
        // the UseOpenTelemetryPrometheusScrapingEndpoint is also commented for same reason as all telemetry will flow from collector when activated as below
        // bascially either telemetry can be send via direct backend or via collector, not both at the same time

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
                    new KeyValuePair<string, object>("service.webapi.environment", "development") // custom attribute
                ]);
        })
        .WithTracing(tracing =>
        {
            tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddGrpcClientInstrumentation()
                .AddConsoleExporter()
                //.AddOtlpExporter(otlpOptions =>
                //{
                //    // Jaegar to visualize traces (open-source)
                //    // https://www.jaegertracing.io/docs/1.62/getting-started/
                //    otlpOptions.Endpoint = new Uri(jaegerEndpoint); 
                //})
                //.AddOtlpExporter(otlpOptions =>
                //{
                //    // Aspire to visualize traces (open-source by microsoft)
                //    // https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/standalone?tabs=bash
                //    otlpOptions.Endpoint = new Uri(aspireEndpoint); 
                //})
                .AddOtlpExporter(otlpOptions =>
                {
                    // Vendor-agnostic way to receive, process and export telemetry data.
                    // Above two are Collector less as the telemetry is directly send to jager and prometheus backends
                    // https://opentelemetry.io/docs/collector/
                    otlpOptions.Endpoint = new Uri(otlpCollectorEndpoint);
                });
        })
        .WithMetrics(metric =>
        {
            // Prometheus to visualize metrics (open-source)
            metric
                .AddMeter(ApplicationDiagnostics.Meter.Name)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                // Metrics provides by .NET
                // https://docs.microsoft.com/en-us/dotnet/core/diagnostics/metrics
                .AddMeter("Microsoft.AspNetCore.Hosting")
                .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                .AddConsoleExporter()
                //.AddPrometheusExporter(); // Prometheus exporter
                // prometheus will scrape metrics from this app at a regular interval based on prometheus configuration in prometheus.yml
                // endpoint in program.cs -> app.UseOpenTelemetryPrometheusScrapingEndpoint();
                .AddOtlpExporter(otlpOptions =>
                {
                    // Vendor-agnostic way to receive, process and export telemetry data.
                    // Above two are Collector less as the telemetry is directly send to jager and prometheus backends
                    // https://opentelemetry.io/docs/collector/
                    otlpOptions.Endpoint = new Uri(otlpCollectorEndpoint);
                });
        });

        return builder;
    }
}
