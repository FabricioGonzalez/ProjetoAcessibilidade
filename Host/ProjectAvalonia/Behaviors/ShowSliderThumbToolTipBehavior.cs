using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace ProjectAvalonia.Behaviors;

public class ShowSliderThumbToolTipBehavior : DisposingBehavior<Control>
{
    public static readonly StyledProperty<Slider?> SliderProperty =
        AvaloniaProperty.Register<ShowSliderThumbToolTipBehavior, Slider?>(name: nameof(Slider));

    public Slider? Slider
    {
        get => GetValue(property: SliderProperty);
        set => SetValue(property: SliderProperty, value: value);
    }

    protected override void OnAttached(
        CompositeDisposable disposables
    )
    {
        if (AssociatedObject is null || Slider is null)
        {
            return;
        }

        Observable
            .FromEventPattern(target: AssociatedObject, eventName: nameof(AssociatedObject.PointerPressed))
            .Subscribe(onNext: _ => SetSliderTooTipIsOpen(isOpen: true))
            .DisposeWith(compositeDisposable: disposables);

        Observable
            .FromEventPattern(target: AssociatedObject, eventName: nameof(AssociatedObject.PointerReleased))
            .Subscribe(onNext: _ => SetSliderTooTipIsOpen(isOpen: false))
            .DisposeWith(compositeDisposable: disposables);

        Observable
            .FromEventPattern(target: AssociatedObject, eventName: nameof(AssociatedObject.PointerCaptureLost))
            .Subscribe(onNext: _ => SetSliderTooTipIsOpen(isOpen: false))
            .DisposeWith(compositeDisposable: disposables);

        Slider.AddHandler(routedEvent: InputElement.PointerPressedEvent, handler: PointerPressed
            , routes: RoutingStrategies.Tunnel);
        Slider.AddHandler(routedEvent: InputElement.PointerReleasedEvent, handler: PointerReleased
            , routes: RoutingStrategies.Tunnel);
        Slider.AddHandler(routedEvent: InputElement.PointerCaptureLostEvent, handler: PointerCaptureLost
            , routes: RoutingStrategies.Tunnel);
    }

    protected override void OnDetaching()
    {
        SetSliderTooTipIsOpen(isOpen: false);

        Slider?.RemoveHandler(routedEvent: InputElement.PointerPressedEvent, handler: PointerPressed);
        Slider?.RemoveHandler(routedEvent: InputElement.PointerReleasedEvent, handler: PointerReleased);
        Slider?.RemoveHandler(routedEvent: InputElement.PointerCaptureLostEvent, handler: PointerCaptureLost);

        base.OnDetaching();
    }

    private void PointerPressed(
        object? sender
        , PointerPressedEventArgs e
    )
    {
        if (sender is not Thumb)
        {
            SetSliderTooTipIsOpen(isOpen: true);
        }
    }

    private void PointerReleased(
        object? sender
        , PointerReleasedEventArgs e
    )
    {
        if (sender is not Thumb)
        {
            SetSliderTooTipIsOpen(isOpen: false);
        }
    }

    private void PointerCaptureLost(
        object? sender
        , PointerCaptureLostEventArgs e
    ) => SetSliderTooTipIsOpen(isOpen: false);

    private void SetSliderTooTipIsOpen(
        bool isOpen
    )
    {
        var thumb = Slider?.GetVisualDescendants()?.OfType<Thumb>().FirstOrDefault();
        if (thumb is not null)
        {
            var toolTip = ToolTip.GetTip(element: thumb);
            if (toolTip is ToolTip)
            {
                thumb.SetValue(property: ToolTip.IsOpenProperty, value: isOpen);
            }
        }
    }
}