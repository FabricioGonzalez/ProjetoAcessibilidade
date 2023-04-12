using System;
using System.Reflection.Metadata;
using QuestPDF.Previewer;

[assembly: MetadataUpdateHandler(handlerType: typeof(HotReloadManager))]

namespace QuestPDF.Previewer;

/// <summary>
///     Helper for subscribing to hot reload notifications.
/// </summary>
public static class HotReloadManager
{
    public static event EventHandler? UpdateApplicationRequested;

    public static void UpdateApplication(
        Type[]? _
    ) => UpdateApplicationRequested?.Invoke(sender: null, e: EventArgs.Empty);
}