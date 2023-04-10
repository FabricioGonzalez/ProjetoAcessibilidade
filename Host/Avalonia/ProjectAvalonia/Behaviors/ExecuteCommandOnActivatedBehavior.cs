using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace ProjectAvalonia.Behaviors;

public class ExecuteCommandOnActivatedBehavior : DisposingBehavior<Control>
{
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<ExecuteCommandOnActivatedBehavior, ICommand?>(name: nameof(Command));

    public ICommand? Command
    {
        get => GetValue(property: CommandProperty);
        set => SetValue(property: CommandProperty, value: value);
    }

    protected override void OnAttached(
        CompositeDisposable disposables
    )
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            var mainWindow = lifetime.MainWindow;

            Observable
                .FromEventPattern(target: mainWindow, eventName: nameof(mainWindow.Activated))
                .Subscribe(onNext: _ =>
                {
                    if (Command is { } cmd && cmd.CanExecute(parameter: default))
                    {
                        cmd.Execute(parameter: default);
                    }
                })
                .DisposeWith(compositeDisposable: disposables);
        }
    }
}