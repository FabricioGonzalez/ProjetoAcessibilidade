using System.Resources;
using System.Threading;

namespace ProjectAvalonia.Common.Extensions;
internal static class ResourceExtensions
{
    private static readonly ResourceManager _resourceLoader = new ResourceManager(typeof(Properties.Resources));

    public static string GetLocalized(this string resourceKey)
    {

        return _resourceLoader.GetString(resourceKey, Thread.CurrentThread.CurrentCulture);
    }
}