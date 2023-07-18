using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Layout;

namespace ProjectAvalonia.Common.Controls;

/// <summary>
///     Container for NavBarItems.
/// </summary>
[PseudoClasses(":horizontal", ":vertical", ":selectable", ":selected")]
public class NavBarItem : Button
{
    public static readonly StyledProperty<IconElement> IconProperty =
        AvaloniaProperty.Register<NavBarItem, IconElement>(name: nameof(Icon));

    public static readonly StyledProperty<Orientation> IndicatorOrientationProperty =
        AvaloniaProperty.Register<NavBarItem, Orientation>(name: nameof(IndicatorOrientation)
            , defaultValue: Orientation.Vertical);

    public static readonly StyledProperty<bool> IsSelectableProperty =
        AvaloniaProperty.Register<NavBarItem, bool>(name: nameof(IsSelectable));

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<NavBarItem, bool>(name: nameof(IsSelected));

    public NavBarItem()
    {
        UpdateIndicatorOrientationPseudoClasses(orientation: IndicatorOrientation);
        UpdatePseudoClass(pseudoClass: ":selectable", value: IsSelectable);
    }

    /// <summary>
    ///     The icon to be shown beside the header text of the item.
    /// </summary>
    public IconElement Icon
    {
        get => GetValue(property: IconProperty);
        set => SetValue(property: IconProperty, value: value);
    }

    /// <summary>
    ///     Gets or sets the indicator orientation.
    /// </summary>
    public Orientation IndicatorOrientation
    {
        get => GetValue(property: IndicatorOrientationProperty);
        set => SetValue(property: IndicatorOrientationProperty, value: value);
    }

    /// <summary>
    ///     Gets or sets flag indicating whether item supports selected state.
    /// </summary>
    public bool IsSelectable
    {
        get => GetValue(property: IsSelectableProperty);
        set => SetValue(property: IsSelectableProperty, value: value);
    }

    /// <summary>
    ///     Gets or sets if the item is selected or not.
    /// </summary>
    public bool IsSelected
    {
        get => GetValue(property: IsSelectedProperty);
        set => SetValue(property: IsSelectedProperty, value: value);
    }

    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs change
    )
    {
        base.OnPropertyChanged(change: change);

        if (change.Property == IndicatorOrientationProperty)
        {
            UpdateIndicatorOrientationPseudoClasses(orientation: change.GetNewValue<Orientation>());
        }

        if (change.Property == IsSelectableProperty)
        {
            UpdatePseudoClass(pseudoClass: ":selectable", value: change.GetNewValue<bool>());
        }

        if (change.Property == IsSelectedProperty)
        {
            UpdatePseudoClass(pseudoClass: ":selected", value: change.GetNewValue<bool>());
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