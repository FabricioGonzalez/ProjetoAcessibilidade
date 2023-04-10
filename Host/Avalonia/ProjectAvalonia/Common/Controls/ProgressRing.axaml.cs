using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace ProjectAvalonia.Common.Controls;

public class ProgressRing : TemplatedControl
{
    public static readonly StyledProperty<bool> IsIndeterminateProperty =
        AvaloniaProperty.Register<ProgressRing, bool>(name: nameof(IsIndeterminate));

    public static readonly StyledProperty<double> PercentageProperty =
        AvaloniaProperty.Register<ProgressRing, double>(name: nameof(Percentage));

    public static readonly StyledProperty<double> StrokeThicknessProperty =
        AvaloniaProperty.Register<ProgressRing, double>(name: nameof(StrokeThickness));

    public bool IsIndeterminate
    {
        get => GetValue(property: IsIndeterminateProperty);
        set => SetValue(property: IsIndeterminateProperty, value: value);
    }

    public double Percentage
    {
        get => GetValue(property: PercentageProperty);
        set => SetValue(property: PercentageProperty, value: value);
    }

    public double StrokeThickness
    {
        get => GetValue(property: StrokeThicknessProperty);
        set => SetValue(property: StrokeThicknessProperty, value: value);
    }

    protected override void OnPropertyChanged<T>(
        AvaloniaPropertyChangedEventArgs<T> e
    )
    {
        base.OnPropertyChanged(change: e);

        if (e.Property == IsIndeterminateProperty)
        {
            PseudoClasses.Set(name: ":indeterminate", value: IsIndeterminate);
        }
    }
}