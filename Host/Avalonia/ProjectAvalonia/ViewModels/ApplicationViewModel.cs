using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

using Avalonia.Controls;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Providers;
using ProjectAvalonia.ViewModels.Dialogs;

using ReactiveUI;

namespace ProjectAvalonia.ViewModels;
public partial class ApplicationViewModel : ViewModelBase, ICanShutdownProvider
{
    private readonly IMainWindowService _mainWindowService;
    [AutoNotify] private bool _isMainWindowShown = true;
    [AutoNotify] private bool _isShuttingDown = false;
    public ApplicationViewModel(IMainWindowService mainWindowService)
    {
        _mainWindowService = mainWindowService;

        QuitCommand = ReactiveCommand.Create(() => Shutdown(false));

        ShowHideCommand = ReactiveCommand.Create(() =>
        {
            if (IsMainWindowShown)
            {
                _mainWindowService.Hide();
            }
            else
            {
                _mainWindowService.Show();
            }
        });

        ShowCommand = ReactiveCommand.Create(() => _mainWindowService.Show());

        AboutCommand = ReactiveCommand.Create(AboutExecute, AboutCanExecute());

        using var bitmap = AssetHelpers.GetBitmapAsset("avares://ProjectAvalonia/Assets/logo.ico");
        TrayIcon = new WindowIcon(bitmap);
    }

    public WindowIcon TrayIcon
    {
        get;
    }
    public ICommand AboutCommand
    {
        get;
    }
    public ICommand ShowCommand
    {
        get;
    }

    public ICommand ShowHideCommand
    {
        get;
    }

    public ICommand QuitCommand
    {
        get;
    }

    private void AboutExecute()
    {
        /*MainViewModel.Instance.DialogScreen.To(
            new AboutViewModel(navigateBack: MainViewModel.Instance.DialogScreen.CurrentPage is not null));*/
    }

    private IObservable<bool> AboutCanExecute()
    {
        return MainViewModel.Instance.DialogScreen
            .WhenAnyValue(x => x.CurrentPage)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Select(x => x is null);
    }

    public void Shutdown(bool restart) => _mainWindowService.Shutdown(restart);

    public void OnShutdownPrevented(bool restartRequest)
    {
        MainViewModel.Instance.ApplyUiConfigWindowSate(); // Will pop the window if it was minimized.
        MainViewModel.Instance.CompactDialogScreen.To(new ShuttingDownViewModel(this, restartRequest));
        IsShuttingDown = true;
    }

    public bool CanShutdown()
    {
        return IsShuttingDown;
    }
}
