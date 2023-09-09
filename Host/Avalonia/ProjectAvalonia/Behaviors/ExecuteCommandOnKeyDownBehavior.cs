using System.Reactive.Disposables;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace ProjectAvalonia.Behaviors;

public class ExecuteCommandOnKeyDownBehavior : AttachedToVisualTreeBehavior<Control>
{
    public static readonly StyledProperty<Key?> KeyProperty =
        AvaloniaProperty.Register<ButtonExecuteCommandOnKeyDownBehavior, Key?>(name: nameof(Key));

    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<ButtonExecuteCommandOnKeyDownBehavior, bool>(name: nameof(IsEnabled)
            , defaultValue: true);

    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<ExecuteCommandOnKeyDownBehavior, ICommand>(name: nameof(Command));

    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<ExecuteCommandOnKeyDownBehavior, object>(name: nameof(CommandParameter));

    public static readonly StyledProperty<RoutingStrategies> EventRoutingStrategyProperty =
        AvaloniaProperty.Register<ExecuteCommandOnKeyDownBehavior, RoutingStrategies>(name: nameof(EventRoutingStrategy)
            , defaultValue: RoutingStrategies.Bubble);

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

    public ICommand Command
    {
        get => GetValue(property: CommandProperty);
        set => SetValue(property: CommandProperty, value: value);
    }

    public object CommandParameter
    {
        get => GetValue(property: CommandParameterProperty);
        set => SetValue(property: CommandParameterProperty, value: value);
    }

    public RoutingStrategies EventRoutingStrategy
    {
        get => GetValue(property: EventRoutingStrategyProperty);
        set => SetValue(property: EventRoutingStrategyProperty, value: value);
    }

    protected override void OnAttachedToVisualTree(
        CompositeDisposable disposable
    )
    {
        var control = AssociatedObject;
        if (control is null)
        {
            return;
        }

        if (control.GetVisualRoot() is IInputElement inputRoot)
        {
            inputRoot.AddHandler(routedEvent: InputElement.KeyDownEvent, handler: RootDefaultKeyDown
                , routes: EventRoutingStrategy);

            disposable.Add(Disposable.Create(() =>
                inputRoot.RemoveHandler(routedEvent: InputElement.KeyDownEvent, handler: RootDefaultKeyDown)));
        }
    }

    private void RootDefaultKeyDown(
        object? sender
        , KeyEventArgs e
    )
    {
        var control = AssociatedObject;
        if (control is null)
        {
            return;
        }

        if (Key is not null && e.Key == Key && control.IsVisible && control.IsEnabled && IsEnabled)
        {
            if (!e.Handled && Command?.CanExecute(parameter: CommandParameter) == true)
            {
                Command.Execute(parameter: CommandParameter);
                e.Handled = true;
            }
        }
    }
}