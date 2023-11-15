using Avalonia;
using Avalonia.Controls;

namespace ProjectAvalonia.Common.Controls;

public class TileControl : ContentControl
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<TileControl, string>(name: nameof(Title));

    public static readonly StyledProperty<object> BottomContentProperty =
        AvaloniaProperty.Register<TileControl, object>(name: nameof(BottomContent));

    public static readonly StyledProperty<bool> IsBottomContentVisibleProperty =
        AvaloniaProperty.Register<TileControl, bool>(name: nameof(IsBottomContentVisible), defaultValue: true);

    public static readonly StyledProperty<Thickness> SeparatorMarginProperty =
        AvaloniaProperty.Register<TileControl, Thickness>(name: nameof(SeparatorMargin));

    public static readonly StyledProperty<double> BottomPartHeightProperty =
        AvaloniaProperty.Register<TileControl, double>(name: nameof(BottomPartHeight));

    public string Title
    {
        get => GetValue(property: TitleProperty);
        set => SetValue(property: TitleProperty, value: value);
    }

    public object BottomContent
    {
        get => GetValue(property: BottomContentProperty);
        set => SetValue(property: BottomContentProperty, value: value);
    }

    public bool IsBottomContentVisible
    {
        get => GetValue(property: IsBottomContentVisibleProperty);
        set => SetValue(property: IsBottomContentVisibleProperty, value: value);
    }

    public Thickness SeparatorMargin
    {
        get => GetValue(property: SeparatorMarginProperty);
        set => SetValue(property: SeparatorMarginProperty, value: value);
    }

    public double BottomPartHeight
    {
        get => GetValue(property: BottomPartHeightProperty);
        set => SetValue(property: BottomPartHeightProperty, value: value);
    }
}