using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ProjectAvalonia.Common.Controls;

public class IconButton : Button
{
    public static readonly StyledProperty<int> IconSizeProperty =
        AvaloniaProperty.Register<IconButton, int>(name: nameof(IconSize), defaultValue: 40);

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<IconButton, string>(name: nameof(Text));

    public static readonly StyledProperty<Geometry> IconProperty =
        AvaloniaProperty.Register<IconButton, Geometry>(name: nameof(Icon));

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