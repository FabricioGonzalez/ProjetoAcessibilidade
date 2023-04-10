using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using Avalonia.Threading;
using Common;
using Core.Entities.Solution;
using Project.Domain.Contracts;
using Project.Domain.Solution.Commands.SolutionItem;
using Project.Domain.Solution.Queries;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.PDFViewer.ViewModels;
using ProjectAvalonia.Features.Project.States.ProjectItems;
using ProjectAvalonia.Logging;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

[NavigationMetaData(
    Title = "Project",
    Caption = "Create and edit projects",
    Order = 0,
    Category = "Project",
    Keywords = new[]
    {
        "Project"
    },
    NavBarPosition = NavBarPosition.Top,
    NavigationTarget = NavigationTarget.HomeScreen,
    IconName = "edit_file_regular")]
public partial class ProjectViewModel : NavBarItemViewModel
{
    private readonly ICommandDispatcher? commandDispatcher;

    private readonly IQueryDispatcher? queryDispatcher;
    [AutoNotify] private string _currentOpenProject = "";

    public ProjectViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();

        SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);

        SelectionMode = NavBarItemSelectionMode.Button;

        projectExplorerViewModel = new ProjectExplorerViewModel();
        projectEditingViewModel = new ProjectEditingViewModel();

        this.WhenAnyValue(property1: vm => vm.projectExplorerViewModel.SelectedItem)
            .WhereNotNull()
            .Subscribe(onNext: item =>
            {
                projectEditingViewModel.SelectedItem = item;
            });

        this.WhenAnyValue(property1: vm => vm.CurrentOpenProject)
            .Where(predicate: prop => !string.IsNullOrWhiteSpace(value: prop))
            .SubscribeAsync(onNextAsync: async prop =>
            {
                (await queryDispatcher.Dispatch<ReadSolutionProjectQuery, Resource<ProjectSolutionModel>>(
                        query: new ReadSolutionProjectQuery(SolutionPath: prop),
                        cancellation: CancellationToken.None))
                    .OnSuccess(
                        onSuccessAction: result =>
                        {
                            Dispatcher
                                .UIThread
                                .Post(action: () =>
                                    projectExplorerViewModel.SolutionModel = result?.Data?.ToSolutionState());
                        })
                    .OnError(onErrorAction: error =>
                    {
                    });
            });

        projectExplorerViewModel.CreateItemCommand = ReactiveCommand.Create<ItemState>(execute: item =>
        {
            projectEditingViewModel.SelectedItem = item;
        });

        projectExplorerViewModel.PrintProjectCommand = ReactiveCommand.Create(execute: () =>
        {
            Navigate(currentTarget: NavigationTarget.FullScreen)
                .To(viewmodel: printPreviewViewModel,
                    mode: NavigationMode.Normal,
                    Parameter: projectExplorerViewModel.SolutionModel);
        }, canExecute: IsSolutionOpened());

        projectExplorerViewModel.OpenSolutionCommand = ReactiveCommand.Create(execute: () =>
        {
        }, canExecute: IsSolutionOpened());

        projectExplorerViewModel.SaveSolutionCommand = ReactiveCommand.CreateFromTask<SolutionStateViewModel>(
            execute: async solution =>
            {
                await commandDispatcher.Dispatch<SyncSolutionCommand, Resource<ProjectSolutionModel>>(
                    command: new SyncSolutionCommand(
                        SolutionData: solution.ToSolutionModel(),
                        SolutionPath: solution.FilePath),
                    cancellation: CancellationToken.None);
            }, canExecute: IsSolutionOpened());

        printPreviewViewModel = new PreviewerViewModel();

        OpenProjectCommand = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var path = await FileDialogHelper.ShowOpenFileDialogAsync(title: "Abrir Projeto");

            if (path is not null)
            {
                CurrentOpenProject = path;
            }
        });

        ((ReactiveCommand<Unit, Unit>)OpenProjectCommand)
            .ThrownExceptions
            .Subscribe(onNext: exception =>
            {
                Logger.LogError(message: "Error!", exception: exception);
            });

        CreateProjectCommand = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var dialogResult = await NavigateDialogAsync(
                dialog: new CreateSolutionViewModel(title: "Criar Solução")
                , target: NavigationTarget.CompactDialogScreen);

            if (dialogResult.Result is { } dialogData)
            {
                IsBusy = true;

                NotificationHelpers.Show(title: "Create", message: "Create Project?", onClick: () =>
                {
                    Logger.LogDebug(message: $"create Project {dialogData.FileName}");
                });

                IsBusy = false;
            }
        });

        ((ReactiveCommand<Unit, Unit>)CreateProjectCommand)
            .ThrownExceptions
            .Subscribe(onNext: exception =>
            {
                Logger.LogError(message: "Error!", exception: exception);
            });
    }

    public ProjectExplorerViewModel projectExplorerViewModel
    {
        get;
    }

    public ProjectEditingViewModel projectEditingViewModel
    {
        get;
    }

    public PreviewerViewModel printPreviewViewModel
    {
        get;
    }

    public ICommand OpenProjectCommand
    {
        get;
    }

    public ICommand CreateProjectCommand
    {
        get;
    }

    protected override void OnNavigatedTo(
        bool isInHistory
        , CompositeDisposable disposables
        , object? Parameter = null
    )
    {
        if (Parameter is string path && !string.IsNullOrWhiteSpace(value: path))
        {
            CurrentOpenProject = path;
        }
    }

    private IObservable<bool> IsSolutionOpened() =>
        this.WhenAnyValue(property1: vm => vm.CurrentOpenProject)
            .Select(selector: prop => !string.IsNullOrWhiteSpace(value: prop));
}