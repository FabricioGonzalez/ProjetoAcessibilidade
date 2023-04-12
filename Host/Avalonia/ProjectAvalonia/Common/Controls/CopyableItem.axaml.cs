using Avalonia;
using Avalonia.Controls;

namespace ProjectAvalonia.Common.Controls;

public class CopyableItem : ContentControl
{
    public static readonly StyledProperty<string?> ContentToCopyProperty =
        AvaloniaProperty.Register<CopyableItem, string?>(name: nameof(ContentToCopy));

    public string? ContentToCopy
    {
        get => GetValue(property: ContentToCopyProperty);
        set => SetValue(property: ContentToCopyProperty, value: value);
    }
}