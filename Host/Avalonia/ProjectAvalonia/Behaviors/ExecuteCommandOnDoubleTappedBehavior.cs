using System.Reactive.Disposables;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace ProjectAvalonia.Behaviors;

public class ExecuteCommandOnDoubleTappedBehavior : DisposingBehavior<Control>
{
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<ExecuteCommandOnDoubleTappedBehavior, ICommand?>(name: nameof(Command));

    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<ExecuteCommandOnDoubleTappedBehavior, object?>(name: nameof(Command));

    public ICommand? Command
    {
        get => GetValue(property: CommandProperty);
        set => SetValue(property: CommandProperty, value: value);
    }

    public object? CommandParameter
    {
        get => GetValue(property: CommandParameterProperty);
        set => SetValue(property: CommandParameterProperty, value: value);
    }

    protected override void OnAttached(
        CompositeDisposable disposables
    ) =>
        Gestures.DoubleTappedEvent.AddClassHandler<InputElement>(
                handler: (
                    x
                    , _
                ) =>
                {
                    if (Equals(objA: x, objB: AssociatedObject))
                    {
                        if (Command is { } cmd && cmd.CanExecute(parameter: default))
                        {
                            if (CommandParameter is not null)
                            {
                                cmd.Execute(parameter: CommandParameter);
                            }
                            else
                            {
                                cmd.Execute(parameter: default);
                            }
                        }
                    }
                },
                routes: RoutingStrategies.Tunnel | RoutingStrategies.Bubble)
            .DisposeWith(compositeDisposable: disposables);
}