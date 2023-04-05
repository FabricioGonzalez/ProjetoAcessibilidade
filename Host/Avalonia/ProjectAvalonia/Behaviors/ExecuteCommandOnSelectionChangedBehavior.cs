using System.Reactive.Disposables;
using System.Windows.Input;

using Avalonia;
using Avalonia.Controls;

namespace ProjectAvalonia.Behaviors;

public class ExecuteCommandOnSelectionChangedBehavior : DisposingBehavior<Control>
{
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<ExecuteCommandOnActivatedBehavior, ICommand?>(nameof(Command));

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    protected override void OnAttached(CompositeDisposable disposables)
    {

    }
}
