using System.Diagnostics;

namespace WebApi.Diagnostics;

public static class ActivityExtensions
{
    public static void SetGreeterName(this Activity activity, string name)
    {
        activity?.SetTag("greeter.name", name);
    }
}
