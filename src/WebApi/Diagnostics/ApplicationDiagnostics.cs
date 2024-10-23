using System.Diagnostics.Metrics;

namespace WebApi.Diagnostics;

public static class ApplicationDiagnostics
{
    private const string ServiceName = "WebApi";

    public static readonly Meter Meter = new(ServiceName);

    public static readonly Counter<int> WeatherForecastCounter = Meter.CreateCounter<int>("WeatherForecastCounter");
}
