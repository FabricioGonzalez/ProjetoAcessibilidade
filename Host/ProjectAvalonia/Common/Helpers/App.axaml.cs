using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Common;
using ProjectAvalonia.ViewModels;
using ReactiveUI;

namespace ProjectAvalonia.Common.Helpers;

public class App : Avalonia.Application
{
    private readonly Func<Task>? _backendInitialiseAsync;
    private readonly bool _startInBg;
    private ApplicationStateManager? _applicationStateManager;

    public App()
    {
        Name = Constants.AppName;
    }

    public App(
        Func<Task> backendInitialiseAsync
        , bool startInBg
    ) : this()
    {
        _startInBg = startInBg;
        _backendInitialiseAsync = backendInitialiseAsync;
    }

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (!Design.IsDesignMode)
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                _applicationStateManager =
                    new ApplicationStateManager(lifetime: desktop, startInBg: _startInBg);

                DataContext = _applicationStateManager.ApplicationViewModel;

                desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                RxApp.MainThreadScheduler.Schedule(
                    async () =>
                    {
                        await _backendInitialiseAsync!(); // Guaranteed not to be null when not in designer.

                        MainViewModel.Instance.Initialize();
                    });
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}