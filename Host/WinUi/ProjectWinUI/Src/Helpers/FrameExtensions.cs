using Microsoft.UI.Xaml.Controls;

namespace ProjectWinUI.Src.Helpers;

public static class FrameExtensions
{
    public static object? GetPageViewModel(
        this Frame frame
    ) => frame?.Content?.GetType().GetProperty(name: "ViewModel")?.GetValue(obj: frame.Content, index: null);
}