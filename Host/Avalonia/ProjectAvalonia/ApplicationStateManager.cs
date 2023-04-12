using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Providers;
using ProjectAvalonia.Logging;
using ProjectAvalonia.State;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.Views;
using ReactiveUI;

namespace ProjectAvalonia;

public class ApplicationStateManager : IMainWindowService
{
    private readonly IClassicDesktopStyleApplicationLifetime _lifetime;

    private readonly StateMachine<State, Trigger> _stateMachine;
    private CompositeDisposable? _compositeDisposable;
    private bool _hideRequest;
    private bool _isShuttingDown;
    private bool _restartRequest;

    internal ApplicationStateManager(
        IClassicDesktopStyleApplicationLifetime lifetime
        , bool startInBg
    )
    {
        _lifetime = lifetime;
        _stateMachine = new StateMachine<State, Trigger>(initialState: State.InitialState);
        ApplicationViewModel = new ApplicationViewModel(mainWindowService: this);

        /* Observable
             .FromEventPattern(ServicesConfig.SingleInstanceChecker, nameof(SingleInstanceChecker.OtherInstanceStarted))
             .ObserveOn(RxApp.MainThreadScheduler)
             .Subscribe(_ => _stateMachine.Fire(Trigger.Show));*/

        _stateMachine.Configure(state: State.InitialState)
            .InitialTransition(state: State.Open)
            .OnTrigger(trigger: Trigger.ShutdownRequested, action: () =>
            {
                if (_restartRequest)
                {
                    AppLifetimeHelper.StartAppWithArgs();
                }

                lifetime.Shutdown();
            })
            .OnTrigger(trigger: Trigger.ShutdownPrevented, action: () =>
            {
                ApplicationViewModel.OnShutdownPrevented(restartRequest: _restartRequest);
                _restartRequest = false; // reset the value.
            });

        _stateMachine.Configure(state: State.Closed)
            .SubstateOf(parent: State.InitialState)
            .OnEntry(action: () =>
            {
                ServicesConfig.UpdateManager.DoUpdateOnClose = true;

                _lifetime.MainWindow.Close();
                _lifetime.MainWindow = null;
                ApplicationViewModel.IsMainWindowShown = false;
            })
            .Permit(trigger: Trigger.Show, state: State.Open)
            .Permit(trigger: Trigger.ShutdownPrevented, state: State.Open)
            .Permit(trigger: Trigger.Loaded, state: State.Open);

        _stateMachine.Configure(state: State.Open)
            .SubstateOf(parent: State.InitialState)
            .OnEntry(action: CreateAndShowMainWindow)
            .Permit(trigger: Trigger.Hide, state: State.Closed)
            .Permit(trigger: Trigger.MainWindowClosed, state: State.Closed)
            .OnTrigger(trigger: Trigger.Show, action: MainViewModel.Instance.ApplyUiConfigWindowSate);

        _lifetime.ShutdownRequested += LifetimeOnShutdownRequested;

        _stateMachine.Start();

        if (!startInBg)
        {
            _stateMachine.Fire(trigger: Trigger.Loaded);
        }
    }

    internal ApplicationViewModel ApplicationViewModel
    {
        get;
    }

    void IMainWindowService.Show() => _stateMachine.Fire(trigger: Trigger.Show);

    void IMainWindowService.Hide()
    {
        _hideRequest = true;
        _stateMachine.Fire(trigger: Trigger.Hide);
    }

    void IMainWindowService.Shutdown(
        bool restart
    )
    {
        _restartRequest = restart;
        _stateMachine.Fire(trigger: ApplicationViewModel.CanShutdown()
            ? Trigger.ShutdownRequested
            : Trigger.ShutdownPrevented);
    }

    private void LifetimeOnShutdownRequested(
        object? sender
        , ShutdownRequestedEventArgs e
    )
    {
        // Shutdown prevention will only work if you directly run the executable.
        e.Cancel = !ApplicationViewModel.CanShutdown();

        Logger.LogDebug(message: $"Cancellation of the shutdown set to: {e.Cancel}.");

        _stateMachine.Fire(trigger: e.Cancel ? Trigger.ShutdownPrevented : Trigger.ShutdownRequested);
    }

    private async Task CheckAction(
        string args
    )
    {
        if (args.Contains(value: "open="))
        {
            ApplicationViewModel.GoToOpenProject(v: args.Split(separator: "=")[1]);
        }

        if (args.Contains(value: "print="))
        {
            await ApplicationViewModel.GoToOpenPrint(v: args.Split(separator: "=")[1]);
        }
    }


    private void CreateAndShowMainWindow()
    {
        if (_lifetime.MainWindow is not null)
        {
            return;
        }

        var result = new MainWindow
        {
            DataContext = MainViewModel.Instance
        };

        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();

        Observable.FromEventPattern<CancelEventArgs>(target: result, eventName: nameof(result.Closing))
            .Select(selector: args => (args.EventArgs, !ApplicationViewModel.CanShutdown()))
            .TakeWhile(predicate: _ => !_isShuttingDown) // Prevents stack overflow.
            .Subscribe(onNext: tup =>
            {
                // _hideRequest flag is used to distinguish what is the user's intent.
                // It is only true when the request comes from the Tray.
                if (ServicesConfig.UiConfig.HideOnClose || _hideRequest)
                {
                    _hideRequest = false; // request processed, set it back to the default.
                    return;
                }

                var (e, preventShutdown) = tup;

                _isShuttingDown = !preventShutdown;
                e.Cancel = preventShutdown;
                _stateMachine.Fire(trigger: preventShutdown ? Trigger.ShutdownPrevented : Trigger.ShutdownRequested);
            })
            .DisposeWith(compositeDisposable: _compositeDisposable);

        Observable.FromEventPattern(target: result, eventName: nameof(result.Closed))
            .Take(count: 1)
            .Subscribe(onNext: _ =>
            {
                _compositeDisposable?.Dispose();
                _compositeDisposable = null;
                _stateMachine.Fire(trigger: Trigger.MainWindowClosed);
            })
            .DisposeWith(compositeDisposable: _compositeDisposable);

        _lifetime.MainWindow = result;

        if (result.WindowState != WindowState.Maximized)
        {
            SetWindowSize(window: result);
        }

        if (_lifetime.Args.Any())
        {
            foreach (var item in _lifetime.Args)
            {
                Task.Run(function: async () => await CheckAction(args: item));
            }
        }


        ObserveWindowSize(window: result, disposables: _compositeDisposable);

        result.Show();

        ApplicationViewModel.IsMainWindowShown = true;
    }

    private void SetWindowSize(
        Window window
    )
    {
        var configWidth = ServicesConfig.UiConfig.WindowWidth;
        var configHeight = ServicesConfig.UiConfig.WindowHeight;
        var currentScreen = window.Screens.ScreenFromPoint(point: window.Position);

        if (configWidth is null || configHeight is null || currentScreen is null)
        {
            return;
        }

        var isValidWidth = configWidth <= currentScreen.WorkingArea.Width && configWidth >= window.MinWidth;
        var isValidHeight = configHeight <= currentScreen.WorkingArea.Height && configHeight >= window.MinHeight;

        if (isValidWidth && isValidHeight)
        {
            window.Width = configWidth.Value;
            window.Height = configHeight.Value;
        }
    }

    private void ObserveWindowSize(
        Window window
        , CompositeDisposable disposables
    ) =>
        window
            .WhenAnyValue(property1: x => x.Bounds)
            .Skip(count: 1)
            .Where(predicate: b => !b.IsEmpty && window.WindowState == WindowState.Normal)
            .Subscribe(onNext: b =>
            {
                ServicesConfig.UiConfig.WindowWidth = b.Width;
                ServicesConfig.UiConfig.WindowHeight = b.Height;
            })
            .DisposeWith(compositeDisposable: disposables);

    private enum Trigger
    {
        Invalid = 0
        , Hide
        , Show
        , Loaded
        , ShutdownPrevented
        , ShutdownRequested
        , MainWindowClosed
    }

    private enum State
    {
        Invalid = 0
        , InitialState
        , Closed
        , Open
    }
}