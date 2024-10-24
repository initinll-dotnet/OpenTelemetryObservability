using System.Diagnostics;

namespace WebApi.Diagnostics;

public static class ActivityExtensions
{
    public static void SetTag(this Activity activity, string key, object value)
    {
        activity?.SetTag(key, value);
    }
}
