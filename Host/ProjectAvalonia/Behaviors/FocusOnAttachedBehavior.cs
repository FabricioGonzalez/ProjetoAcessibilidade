using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

namespace ProjectAvalonia.Behaviors;

public class FocusOnAttachedBehavior : AttachedToVisualTreeBehavior<Control>
{
    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<FocusOnAttachedBehavior, bool>(name: nameof(IsEnabled), defaultValue: true);

    public bool IsEnabled
    {
        get => GetValue(property: IsEnabledProperty);
        set => SetValue(property: IsEnabledProperty, value: value);
    }

    protected override void OnAttachedToVisualTree(
        CompositeDisposable disposables
    )
    {
        if (IsEnabled)
        {
            Dispatcher.UIThread.Post(action: () => AssociatedObject?.Focus());
        }
    }
}