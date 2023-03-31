using System.Reactive.Disposables;
using System.Windows.Input;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace ProjectAvalonia.Behaviors;

public class ExecuteCommandOnTappedBehavior : DisposingBehavior<Control>
{
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<ExecuteCommandOnTappedBehavior, ICommand?>(nameof(Command));

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<object?> CommandParameterProperty =
    AvaloniaProperty.Register<ExecuteCommandOnTappedBehavior, object?>(nameof(Command));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    protected override void OnAttached(CompositeDisposable disposables)
    {
        Gestures.TappedEvent.AddClassHandler<InputElement>(
                (x, _) =>
                {
                    if (Equals(x, AssociatedObject))
                    {
                        if (Command is { } cmd && cmd.CanExecute(default))
                        {
                            if (CommandParameter is not null)
                                cmd.Execute(CommandParameter);
                            else
                                cmd.Execute(default);
                        }
                    }
                },
                RoutingStrategies.Tunnel | RoutingStrategies.Bubble)
            .DisposeWith(disposables);
    }
}
