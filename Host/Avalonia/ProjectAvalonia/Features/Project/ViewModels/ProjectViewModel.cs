using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;

using Avalonia.Threading;

using Common;

using Core.Entities.Solution;
using Core.Entities.Solution.ItemsGroup;

using Project.Application.Contracts;
using Project.Application.Solution.Commands.SyncSolutionCommands;
using Project.Application.Solution.Queries;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.PDFViewer.ViewModels;
using ProjectAvalonia.Logging;

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
    [AutoNotify] private string _currentOpenProject = "";
    /*[AutoNotify] private SolutionStateViewModel _solutionModel;*/

    private readonly IQueryDispatcher? queryDispatcher;
    private readonly ICommandDispatcher? commandDispatcher;

    public ProjectViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();

        SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);

        SelectionMode = NavBarItemSelectionMode.Button;

        projectExplorerViewModel = new ProjectExplorerViewModel();
        projectEditingViewModel = new ProjectEditingViewModel();

        projectExplorerViewModel
            .WhenAnyValue(vm => vm.SelectedItem)
            .WhereNotNull()
            .Subscribe(item =>
            {
                projectEditingViewModel.SelectedItem = item;
            });

        projectExplorerViewModel.CreateItemCommand = ReactiveCommand.Create<ItemModel>(execute: (item) =>
        {
            projectEditingViewModel.SelectedItem = item;
        });

        projectExplorerViewModel.PrintProjectCommand = ReactiveCommand.Create(execute: () =>
        {
            Navigate(NavigationTarget.FullScreen)
            .To(printPreviewViewModel,
            ProjectAvalonia.ViewModels.Navigation.NavigationMode.Normal,
            projectExplorerViewModel.SolutionModel);
        }, canExecute: IsSolutionOpened());

        projectExplorerViewModel.OpenSolutionCommand = ReactiveCommand.Create(execute: () =>
        {


        }, canExecute: IsSolutionOpened());

        projectExplorerViewModel.SaveSolutionCommand = ReactiveCommand.CreateFromTask<SolutionStateViewModel>(execute: async (solution) =>
        {
            await commandDispatcher.Dispatch<SyncSolutionCommand, Resource<ProjectSolutionModel>>(
                command: new(
                solutionData: solution.ToSolutionModel(),
                solutionPath: solution.FilePath),
                cancellation: CancellationToken.None);
        }, canExecute: IsSolutionOpened());

        printPreviewViewModel = new PreviewerViewModel();

        OpenProjectCommand = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var path = await FileDialogHelper.ShowOpenFileDialogAsync("Abrir Projeto");

            if (path is not null)
            {
                CurrentOpenProject = path;

                (await queryDispatcher.Dispatch<ReadSolutionProjectQuery, Resource<ProjectSolutionModel>>(
                    query: new(solutionPath: path),
                    cancellation: CancellationToken.None))
                    .OnSuccess(
                    (result) =>
                    {
                        Dispatcher
                        .UIThread
                        .Post(() => projectExplorerViewModel.SolutionModel = result.Data.ToSolutionState());
                    })
                    .OnError(error =>
                    {

                    })
                    .OnLoadingStarted(loading =>
                    {

                    });

            }
        });
        ((ReactiveCommand<Unit, Unit>)OpenProjectCommand)
            .ThrownExceptions
            .Subscribe(exception =>
        {
            Logger.LogError("Error!", exception);
        });

        CreateProjectCommand = ReactiveCommand.CreateFromTask(execute: async () =>
        {
            var dialogResult = await NavigateDialogAsync(
            new CreateSolutionViewModel("Criar Solução")
            , NavigationTarget.CompactDialogScreen);

            if (dialogResult.Result is { } dialogData)
            {
                IsBusy = true;

                NotificationHelpers.Show(title: "Create", "Create Project?", () =>
                {
                    Logger.LogDebug($"create Project {dialogData.FileName}");
                });

                IsBusy = false;
            }
        });
        ((ReactiveCommand<Unit, Unit>)CreateProjectCommand)
           .ThrownExceptions
           .Subscribe(exception =>
           {
               Logger.LogError("Error!", exception);
           });
    }

    private IObservable<bool> IsSolutionOpened()
    {
        return this.WhenAnyValue(vm => vm.CurrentOpenProject)
            .Select(prop => !string.IsNullOrWhiteSpace(prop));
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
}
