using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace ProjectAvalonia.Behaviors;

public class ButtonExecuteCommandOnKeyDownBehavior : AttachedToVisualTreeBehavior<Button>
{
    public static readonly StyledProperty<Key?> KeyProperty =
        AvaloniaProperty.Register<ButtonExecuteCommandOnKeyDownBehavior, Key?>(name: nameof(Key));

    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<ButtonExecuteCommandOnKeyDownBehavior, bool>(name: nameof(IsEnabled));

    public Key? Key
    {
        get => GetValue(property: KeyProperty);
        set => SetValue(property: KeyProperty, value: value);
    }

    public bool IsEnabled
    {
        get => GetValue(property: IsEnabledProperty);
        set => SetValue(property: IsEnabledProperty, value: value);
    }

    protected override void OnAttachedToVisualTree(
        CompositeDisposable disposable
    )
    {
        var button = AssociatedObject;
        if (button is null)
        {
            return;
        }

        if (button.GetVisualRoot() is IInputElement inputRoot)
        {
            inputRoot.AddHandler(routedEvent: InputElement.KeyDownEvent, handler: RootDefaultKeyDown);

            disposable.Add(Disposable.Create(() =>
                inputRoot.RemoveHandler(routedEvent: InputElement.KeyDownEvent, handler: RootDefaultKeyDown)));
        }
    }

    private void RootDefaultKeyDown(
        object? sender
        , KeyEventArgs e
    )
    {
        var button = AssociatedObject;
        if (button is null)
        {
            return;
        }

        if (Key is not null && e.Key == Key && button.IsVisible && button.IsEnabled && IsEnabled)
        {
            if (!e.Handled && button.Command?.CanExecute(parameter: button.CommandParameter) == true)
            {
                button.Command.Execute(parameter: button.CommandParameter);
                e.Handled = true;
            }
        }
    }
}