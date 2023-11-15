using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace ProjectAvalonia.Behaviors;

internal class HorizontalScrollViewerBehavior : Behavior<ScrollViewer>
{
    public enum ChangeSize
    {
        Line
        , Page
    }

    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<HorizontalScrollViewerBehavior, bool>(name: nameof(IsEnabled), defaultValue: true);

    public static readonly StyledProperty<bool> RequireShiftKeyProperty =
        AvaloniaProperty.Register<HorizontalScrollViewerBehavior, bool>(name: nameof(RequireShiftKey));

    public static readonly StyledProperty<ChangeSize> ScrollChangeSizeProperty =
        AvaloniaProperty.Register<HorizontalScrollViewerBehavior, ChangeSize>(name: nameof(ScrollChangeSize));

    public bool IsEnabled
    {
        get => GetValue(property: IsEnabledProperty);
        set => SetValue(property: IsEnabledProperty, value: value);
    }

    public bool RequireShiftKey
    {
        get => GetValue(property: RequireShiftKeyProperty);
        set => SetValue(property: RequireShiftKeyProperty, value: value);
    }

    public ChangeSize ScrollChangeSize
    {
        get => GetValue(property: ScrollChangeSizeProperty);
        set => SetValue(property: ScrollChangeSizeProperty, value: value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject!.AddHandler(routedEvent: InputElement.PointerWheelChangedEvent, handler: OnPointerWheelChanged
            , routes: RoutingStrategies.Tunnel);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject!.RemoveHandler(routedEvent: InputElement.PointerWheelChangedEvent
            , handler: OnPointerWheelChanged);
    }

    private void OnPointerWheelChanged(
        object? sender
        , PointerWheelEventArgs e
    )
    {
        if (!IsEnabled)
        {
            e.Handled = true;
            return;
        }

        if ((RequireShiftKey && e.KeyModifiers == KeyModifiers.Shift) || !RequireShiftKey)
        {
            if (e.Delta.Y < 0)
            {
                if (ScrollChangeSize == ChangeSize.Line)
                {
                    AssociatedObject!.LineRight();
                }
                else
                {
                    AssociatedObject!.PageRight();
                }
            }
            else
            {
                if (ScrollChangeSize == ChangeSize.Line)
                {
                    AssociatedObject!.LineLeft();
                }
                else
                {
                    AssociatedObject!.PageLeft();
                }
            }
        }
    }
}