using Avalonia;
using Avalonia.Controls;

namespace ProjectAvalonia.Common.Controls;

public class InfoMessage : Label
{
    public static readonly StyledProperty<int> IconSizeProperty =
        AvaloniaProperty.Register<InfoMessage, int>(name: nameof(IconSize), defaultValue: 20);

    public int IconSize
    {
        get => GetValue(property: IconSizeProperty);
        set => SetValue(property: IconSizeProperty, value: value);
    }
}