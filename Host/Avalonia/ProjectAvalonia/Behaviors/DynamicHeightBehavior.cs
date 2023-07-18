using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

public class DynamicHeightBehavior : DisposingBehavior<Control>
{
    public static readonly StyledProperty<double> HeightMultiplierProperty =
        AvaloniaProperty.Register<DynamicHeightBehavior, double>(name: nameof(HeightMultiplier));

    public static readonly StyledProperty<double> HideThresholdHeightProperty =
        AvaloniaProperty.Register<DynamicHeightBehavior, double>(name: nameof(HideThresholdHeight));

    public double HeightMultiplier
    {
        get => GetValue(property: HeightMultiplierProperty);
        set => SetValue(property: HeightMultiplierProperty, value: value);
    }

    public double HideThresholdHeight
    {
        get => GetValue(property: HideThresholdHeightProperty);
        set => SetValue(property: HideThresholdHeightProperty, value: value);
    }

    protected override void OnAttached(
        CompositeDisposable disposables
    ) =>
        AssociatedObject?.Parent?.WhenAnyValue(property1: x => ((Control)x).Bounds)
            .Subscribe(onNext: bounds =>
            {
                var newHeight = bounds.Height * HeightMultiplier;

                if (newHeight < HideThresholdHeight)
                {
                    AssociatedObject.IsVisible = false;
                }
                else
                {
                    AssociatedObject.IsVisible = true;
                    AssociatedObject.Height = newHeight;
                }
            })
            .DisposeWith(compositeDisposable: disposables);
}