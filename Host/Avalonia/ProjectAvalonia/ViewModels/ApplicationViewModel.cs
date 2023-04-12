using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Common;
using Core.Entities.Solution;
using Project.Domain.Contracts;
using Project.Domain.Solution.Queries;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Mappers;
using ProjectAvalonia.Common.Providers;
using ProjectAvalonia.ViewModels.Dialogs;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.ViewModels;

public partial class ApplicationViewModel
    : ViewModelBase
        , ICanShutdownProvider
{
    private readonly IMainWindowService _mainWindowService;
    private readonly IQueryDispatcher _queryDispatcher;

    [AutoNotify] private bool _isMainWindowShown = true;
    [AutoNotify] private bool _isShuttingDown;

    public ApplicationViewModel(
        IMainWindowService mainWindowService
    )
    {
        _queryDispatcher = Locator.Current.GetService<IQueryDispatcher>();
        _mainWindowService = mainWindowService;

        QuitCommand = ReactiveCommand.Create(execute: () => Shutdown(restart: false));

        ShowHideCommand = ReactiveCommand.Create(execute: () =>
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

        ShowCommand = ReactiveCommand.Create(execute: () => _mainWindowService.Show());

        AboutCommand = ReactiveCommand.Create(execute: AboutExecute, canExecute: AboutCanExecute());

        using var bitmap = AssetHelpers.GetBitmapAsset(path: "avares://ProjectAvalonia/Assets/logo.ico");
        TrayIcon = new WindowIcon(bitmap: bitmap);
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

    public bool CanShutdown() => IsShuttingDown;

    private void AboutExecute()
    {
        /*MainViewModel.Instance.DialogScreen.To(
            new AboutViewModel(navigateBack: MainViewModel.Instance.DialogScreen.CurrentPage is not null));*/
    }

    private IObservable<bool> AboutCanExecute() =>
        MainViewModel.Instance.DialogScreen
            .WhenAnyValue(property1: x => x.CurrentPage)
            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
            .Select(selector: x => x is null);

    public void Shutdown(
        bool restart
    ) => _mainWindowService.Shutdown(restart: restart);

    public void OnShutdownPrevented(
        bool restartRequest
    )
    {
        MainViewModel.Instance.ApplyUiConfigWindowSate(); // Will pop the window if it was minimized.
        MainViewModel.Instance.CompactDialogScreen.To(
            viewmodel: new ShuttingDownViewModel(applicationViewModel: this, restart: restartRequest));
        IsShuttingDown = true;
    }

    internal async Task GoToOpenPrint(
        string v
    ) =>
        (await _queryDispatcher.Dispatch<ReadSolutionProjectQuery, Resource<ProjectSolutionModel>>(
            query: new ReadSolutionProjectQuery(SolutionPath: v),
            cancellation: CancellationToken.None))
        .OnSuccess(
            onSuccessAction: async result =>
            {
                MainViewModel.Instance.PrintProject(solutionState: result?.Data?.ToSolutionState());

                /*  MainViewModel.Instance.FullScreen.To(await NavigationManager.MaterialiseViewModelAsync(PreviewerViewModel.MetaData)
                    , Parameter: result.Data.ToSolutionState());

          MainViewModel.Instance.MainScreen.To(
                      await NavigationManager.MaterialiseViewModelAsync(ProjectViewModel.MetaData)
                     , Parameter: v);*/
            })
        .OnError(onErrorAction: error =>
        {
        });

    internal void GoToOpenProject(
        string v
    ) => MainViewModel.Instance.OpenProject(projectPath: v);
}