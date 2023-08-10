using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Application.Solution.Contracts;
using Avalonia.Threading;
using Domain.Solutions;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.PDFViewer.ViewModels;
using ProjectAvalonia.Models;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.ProjectItems;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

[NavigationMetaData(
    Title = "Projetos",
    Caption = "Criar e editar projetos",
    Order = 0,
    LocalizedTitle = "ProjectViewNavLabel",
    Category = "Projetos",
    Keywords = new[]
    {
        "Projeto"
    },
    NavBarPosition = NavBarPosition.Top,
    NavigationTarget = NavigationTarget.HomeScreen,
    IconName = "edit_file_regular")]
public partial class ProjectViewModel
    : NavBarItemViewModel
        , IProjectViewModel
{
    private readonly ObservableAsPropertyHelper<bool> _isSolutionOpen;

    /*private readonly IMediator _mediator;*/
    private readonly ISolutionService _solutionService;

    /*private readonly ProjectSolutionModel? projectSolution;*/

    public ProjectViewModel()
    {
        SetupCancel(
            enableCancel: false,
            enableCancelOnEscape: true,
            enableCancelOnPressed: true);

        var isSolutionOpen = this.WhenAnyValue(vm => vm.IsSolutionOpen);

        var explorer = this.WhenAnyValue(vm => vm.ProjectExplorerViewModel);

        explorer
            .Select(x => x?.SolutionRootItem.LocationItems.Count > 0 || x?.SolutionState is not null)
            .ToProperty(
                source: this,
                property: x => x.IsSolutionOpen,
                result: out _isSolutionOpen);

        ToolBar = new MenuViewModel(CreateMenu(isSolutionOpen).ToImmutable());


        ProjectInteractions.SyncSolutionInteraction.RegisterHandler(
            context =>
            {
                _ = SaveSolutionCommand?.Execute();

                context.SetOutput(Unit.Default);
            });

        SelectionMode = NavBarItemSelectionMode.Button;

        /*_mediator = mediator;*/

        OpenProjectCommand = ReactiveCommand.CreateFromTask(OpenSolution);

        CreateProjectCommand = ReactiveCommand.CreateFromTask(CreateSolution);

        PrintProjectCommand = ReactiveCommand.Create(
            execute: PrintSolution,
            canExecute: isSolutionOpen);

        ProjectEditingViewModel = new ProjectEditingViewModel();
        ProjectPrintPreviewViewModel = new PreviewerViewModel();


        EnableAutoBusyOn(
            OpenProjectCommand,
            CreateProjectCommand);
    }

    public ProjectViewModel(
        ISolutionService solutionService
        /*, IMediator mediator*/
    ) : this()
    {
        _solutionService = solutionService;
    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;

    public PreviewerViewModel ProjectPrintPreviewViewModel
    {
        get;
    }

    public override MenuViewModel? ToolBar
    {
        get;
    }

    public bool IsSolutionOpen => _isSolutionOpen.Value;

    public IProjectExplorerViewModel ProjectExplorerViewModel
    {
        get;
        private set;
    }

    public IProjectEditingViewModel ProjectEditingViewModel
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> OpenProjectCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> CreateProjectCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> PrintProjectCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> SaveSolutionCommand
    {
        get;
    }

    private ImmutableList<IMenuItem>.Builder CreateMenu(
        IObservable<bool> isSolutionOpen
    )
    {
        var listBuilder = ImmutableList.CreateBuilder<IMenuItem>();

        listBuilder.Add(new MenuItemModel(
            label: "Solution_Open_ToolBarItem".GetLocalized(),
            command: ReactiveCommand.CreateFromTask(OpenSolution),
            icon: "file_open_24_rounded".GetIcon(),
            gesture: "Ctrl+Shift+O"));

        listBuilder.Add(new MenuItemModel(
            label: "Solution_Create_ToolBarItem".GetLocalized(),
            command: ReactiveCommand.CreateFromTask(CreateSolution),
            icon: "solution_create_24_rounded".GetIcon(),
            gesture: "Ctrl+Shift+N"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
            label: "Project_Item_Save_ToolBarItem".GetLocalized(),
            command: ReactiveCommand.CreateFromTask(
                execute: SaveReportData,
                canExecute: isSolutionOpen),
            icon: "save_data_24_rounded".GetIcon(),
            gesture: "Ctrl+S"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
            label: "Solution_Print_ToolBarItem".GetLocalized(),
            command: ReactiveCommand.Create(
                execute: PrintSolution,
                canExecute: isSolutionOpen),
            icon: "print_24_rounded".GetIcon(),
            gesture: "Ctrl+P"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
            label: "Project_Add_Folder_ToolBarItem".GetLocalized(),
            command: ReactiveCommand.Create(
                execute: () =>
                {
                    _ = ProjectExplorerViewModel?.CreateFolderCommand?.Execute().Subscribe();
                },
                canExecute: isSolutionOpen),
            icon: "add_folder_24_rounded".GetIcon(),
            gesture: "Ctrl+Shift+N"));

        return listBuilder;
    }

    private async Task SaveReportData()
    {
        if (ProjectEditingViewModel is { SelectedItem: not null })
        {
            await ProjectEditingViewModel.SelectedItem.SaveItemCommand.Execute();
        }
    }

    private void PrintSolution() =>
        Navigate(NavigationTarget.FullScreen)
            .To(
                viewmodel: ProjectPrintPreviewViewModel,
                mode: NavigationMode.Normal,
                Parameter: ProjectExplorerViewModel.SolutionState);

    private async Task OpenSolution() =>
        /*var path = await FileDialogHelper.ShowOpenFileDialogAsync("Abrir Projeto");*/
        await (await FileDialogHelper.GetFileAsync())
            .Match(Succ: async file =>
                {
                    if (file is { Name: { Length: > 0 } })
                    {
                        await ReadSolutionAndOpen(file.Path.LocalPath);
                    }
                },
                Fail: error => Task.CompletedTask);

    private async Task ReadSolutionAndOpen(
        string path
    )
    {
        var result = await _solutionService.OpenSolution(path);

        Dispatcher
            .UIThread
            .Post(
                () =>
                {
                    ProjectExplorerViewModel = new ProjectExplorerViewModel(
                        result.ToSolutionState()
                    )
                    {
                        SetEditingItem = ReactiveCommand.Create<IItemViewModel>(item =>
                            ((ProjectEditingViewModel)ProjectEditingViewModel).AddItemToEdit.Execute(item)
                            .Subscribe())
                    };
                    this.RaisePropertyChanged(nameof(ProjectExplorerViewModel));
                });
    }

    private async Task CreateSolution()
    {
        var dialogResult = await NavigateDialogAsync(
            dialog: new CreateSolutionViewModel(title: "Criar Solução")
            , target: NavigationTarget.CompactDialogScreen);

        if (dialogResult is { Kind: DialogResultKind.Normal } result)

        {
            await _solutionService.SaveSolution(path: result.Result.local, solutionToSave: result.Result.solution);
            Dispatcher.UIThread.Post(() =>
            {
                /*ProjectExplorerViewModel = new ProjectExplorerViewModel();*/
            });
        }
        /*var dialogResult = await NavigateDialogAsync(
            dialog: new CreateSolutionViewModel(title: "Criar Solução",
                solutionState: ProjectExplorerViewModel?.SolutionState ??
                               ProjectSolutionModel.Create("", new SolutionInfo())
            )
            , target: NavigationTarget.CompactDialogScreen);

        if (dialogResult is { Result: { } dialogData, Kind: DialogResultKind.Normal })
        {
            _ = await _mediator.Send(
                request: new CreateSolutionCommand(SolutionPath: dialogData.FilePath, SolutionData: dialogData)
                , cancellation: CancellationToken.None);

            Dispatcher
                .UIThread
                .Post(
                    () =>
                    {
                        ProjectExplorerViewModel = new ProjectExplorerViewModel(
                            dialogData
                        )
                        {
                            SetEditingItem = ReactiveCommand.Create<IItemViewModel>(item =>
                                ((ProjectEditingViewModel)ProjectEditingViewModel).AddItemToEdit.Execute(item)
                                .Subscribe())
                        };
                        this.RaisePropertyChanged(nameof(ProjectExplorerViewModel));
                    });
    }
                    */
    }

    protected override void OnNavigatedTo(
        bool isInHistory
        , CompositeDisposable disposables
        , object? Parameter = null
    )
    {
        if (Parameter is string { Length: > 0 } path)
        {
            Dispatcher.UIThread.InvokeAsync(
                async () =>
                {
                    await ReadSolutionAndOpen(path);
                });
        }
    }
}

public static class ProjectInteractions
{
    public static Interaction<Unit, Unit> SyncSolutionInteraction = new();
}

public static class Mapping
{
    public static SolutionState ToSolutionState(
        this ISolution solution
    ) =>
        new()
        {
            LocationItems = new ObservableCollection<LocationItemState>(solution.ProjectItems.GroupBy(it => it.Locale)
                .Select(it => new LocationItemState
                {
                    Name = it.Key, ItemGroup = new ObservableCollection<ItemGroupState>(it.GroupBy(it2 => it2.Group)
                        .Select(it3 => new ItemGroupState
                        {
                            Name = it3.Key, ItemStates = it3.Select(it4 => new ItemState { Name = it4.Name })
                        }))
                }))
        };
}