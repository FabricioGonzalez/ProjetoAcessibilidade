using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ProjectAvalonia.Common.Controls;

public class TileButton : Button
{
    public static readonly StyledProperty<int> IconSizeProperty =
        AvaloniaProperty.Register<TileButton, int>(name: nameof(IconSize), defaultValue: 40);

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<TileButton, string>(name: nameof(Text));

    public static readonly StyledProperty<Geometry> IconProperty =
        AvaloniaProperty.Register<TileButton, Geometry>(name: nameof(Icon));

    public int IconSize
    {
        get => GetValue(property: IconSizeProperty);
        set => SetValue(property: IconSizeProperty, value: value);
    }

    public string Text
    {
        get => GetValue(property: TextProperty);
        set => SetValue(property: TextProperty, value: value);
    }

    public Geometry Icon
    {
        get => GetValue(property: IconProperty);
        set => SetValue(property: IconProperty, value: value);
    }
}