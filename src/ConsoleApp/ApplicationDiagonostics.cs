using System.Diagnostics;

namespace ConsoleApp;

public static class ApplicationDiagonostics
{
    public const string ActivitySourceName = "Console.App.Diagnostics";
    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
}