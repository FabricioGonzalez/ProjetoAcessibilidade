using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Input;
using Avalonia.Layout;

namespace ProjectAvalonia.Common.Controls;

/// <summary>
///     Container for NavBarItems.
/// </summary>
[PseudoClasses(":horizontal", ":vertical", ":selected")]
public class NavBarItem : ContentControl
{
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<NavBarItem, ICommand?>(nameof(Command));

    public static readonly StyledProperty<IconElement> IconProperty =
        AvaloniaProperty.Register<NavBarItem, IconElement>(nameof(Icon));

    public static readonly StyledProperty<Orientation> IndicatorOrientationProperty =
        AvaloniaProperty.Register<NavBarItem, Orientation>(name: nameof(IndicatorOrientation)
            , defaultValue: Orientation.Vertical);

    public NavBarItem()
    {
        UpdateIndicatorOrientationPseudoClasses(IndicatorOrientation);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(property: CommandProperty, value: value);
    }

    /// <summary>
    ///     The icon to be shown beside the header text of the item.
    /// </summary>
    public IconElement Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(property: IconProperty, value: value);
    }

    /// <summary>
    ///     Gets or sets the indicator orientation.
    /// </summary>
    public Orientation IndicatorOrientation
    {
        get => GetValue(IndicatorOrientationProperty);
        set => SetValue(property: IndicatorOrientationProperty, value: value);
    }

    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs change
    )
    {
        base.OnPropertyChanged(change);

        if (change.Property == IndicatorOrientationProperty)
        {
            UpdateIndicatorOrientationPseudoClasses(change.GetNewValue<Orientation>());
        }
    }

    protected override void OnPointerPressed(
        PointerPressedEventArgs e
    )
    {
        base.OnPointerPressed(e);

        if (Command != null && Command.CanExecute(default))
        {
            Command.Execute(default);
        }
    }

    private void UpdateIndicatorOrientationPseudoClasses(
        Orientation orientation
    )
    {
        PseudoClasses.Set(name: ":horizontal", value: orientation == Orientation.Horizontal);
        PseudoClasses.Set(name: ":vertical", value: orientation == Orientation.Vertical);
    }

    private void UpdatePseudoClass(
        string pseudoClass
        , bool value
    ) => PseudoClasses.Set(name: pseudoClass, value: value);
}