using System.Resources;
using System.Threading;
using ProjectAvalonia.Properties;

namespace ProjectAvalonia.Common.Extensions;

internal static class ResourceExtensions
{
    private static readonly ResourceManager _resourceLoader = new(resourceSource: typeof(Resources));

    public static string GetLocalized(
        this string resourceKey
    ) => _resourceLoader?.GetString(name: resourceKey, culture: Thread.CurrentThread.CurrentCulture) ?? "";

    public static bool HasProperty(
        this string resourceKey
    ) => _resourceLoader.GetString(name: resourceKey, culture: Thread.CurrentThread.CurrentCulture) is not null;
}