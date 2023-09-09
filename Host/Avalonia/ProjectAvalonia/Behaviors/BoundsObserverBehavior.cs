using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace ProjectAvalonia.Behaviors;

internal class BoundsObserverBehavior : DisposingBehavior<Control>
{
    public static readonly StyledProperty<Rect> BoundsProperty =
        AvaloniaProperty.Register<BoundsObserverBehavior, Rect>(name: nameof(Bounds)
            , defaultBindingMode: BindingMode.OneWay);

    public static readonly StyledProperty<double> WidthProperty =
        AvaloniaProperty.Register<BoundsObserverBehavior, double>(name: nameof(Width)
            , defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<double> HeightProperty =
        AvaloniaProperty.Register<BoundsObserverBehavior, double>(name: nameof(Height)
            , defaultBindingMode: BindingMode.TwoWay);

    public Rect Bounds
    {
        get => GetValue(property: BoundsProperty);
        set => SetValue(property: BoundsProperty, value: value);
    }

    public double Width
    {
        get => GetValue(property: WidthProperty);
        set => SetValue(property: WidthProperty, value: value);
    }

    public double Height
    {
        get => GetValue(property: HeightProperty);
        set => SetValue(property: HeightProperty, value: value);
    }

    protected override void OnAttached(
        CompositeDisposable disposables
    )
    {
        if (AssociatedObject is not null)
        {
            disposables.Add(item: this.GetObservable(property: BoundsProperty)
                .Subscribe(onNext: bounds =>
                {
                    Width = bounds.Width;
                    Height = bounds.Height;
                }));
        }
    }
}