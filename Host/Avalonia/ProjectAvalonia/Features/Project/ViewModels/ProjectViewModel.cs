using System;
using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.PDFViewer.ViewModels;
using ProjectAvalonia.Models;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;
using ProjetoAcessibilidade.Core.Entities.Solution;
using ProjetoAcessibilidade.Core.Entities.Solution.ReportInfo;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Solution.Commands.SolutionItem;
using ProjetoAcessibilidade.Domain.Solution.Queries;
using ReactiveUI;
using Splat;

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

    private readonly IMediator _mediator;

    private readonly ProjectSolutionModel? projectSolution;

    public ProjectViewModel()
    {
        SetupCancel(
            false,
            true,
            true);

        var isSolutionOpen = this.WhenAnyValue(vm => vm.IsSolutionOpen);

        _ = this.WhenAnyValue(vm => vm.ProjectExplorerViewModel)
            .Select(x => x?.SolutionRootItem.ItemsGroups.Count > 0 || x?.SolutionState is not null)
            .ToProperty(
                this,
                x => x.IsSolutionOpen,
                out _isSolutionOpen);

        ToolBar = new MenuViewModel(CreateMenu(isSolutionOpen).ToImmutable());


        _ = ProjectInteractions.SyncSolutionInteraction.RegisterHandler(
            context =>
            {
                _ = SaveSolutionCommand?.Execute();

                context.SetOutput(Unit.Default);
            });

        SelectionMode = NavBarItemSelectionMode.Button;

        _mediator ??= Locator.Current.GetService<IMediator>();

        OpenProjectCommand = ReactiveCommand.CreateFromTask(OpenSolution);

        CreateProjectCommand = ReactiveCommand.CreateFromTask(CreateSolution);

        PrintProjectCommand = ReactiveCommand.Create(
            PrintSolution,
            isSolutionOpen);

        ProjectEditingViewModel = new ProjectEditingViewModel();
        ProjectPrintPreviewViewModel = new PreviewerViewModel();

        EnableAutoBusyOn(
            OpenProjectCommand,
            CreateProjectCommand);
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
            "Solution_Open_ToolBarItem".GetLocalized(),
            ReactiveCommand.CreateFromTask(OpenSolution),
            "file_open_24_rounded".GetIcon(),
            "Ctrl+Shift+O"));

        listBuilder.Add(new MenuItemModel(
            "Solution_Create_ToolBarItem".GetLocalized(),
            ReactiveCommand.CreateFromTask(CreateSolution),
            "solution_create_24_rounded".GetIcon(),
            "Ctrl+Shift+N"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
            "Project_Item_Save_ToolBarItem".GetLocalized(),
            ReactiveCommand.CreateFromTask(
                SaveReportData,
                isSolutionOpen),
            "save_data_24_rounded".GetIcon(),
            "Ctrl+S"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
            "Solution_Print_ToolBarItem".GetLocalized(),
            ReactiveCommand.Create(
                PrintSolution,
                isSolutionOpen),
            "print_24_rounded".GetIcon(),
            "Ctrl+P"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
            "Project_Add_Folder_ToolBarItem".GetLocalized(),
            ReactiveCommand.Create(
                () =>
                {
                    _ = ProjectExplorerViewModel?.CreateFolderCommand?.Execute().Subscribe();
                },
                isSolutionOpen),
            "add_folder_24_rounded".GetIcon(),
            "Ctrl+Shift+N"));


        /* listBuilder.Add(new MenuItemModel(
          label: "Add File",
          command: ReactiveCommand.Create(
           execute: () =>
           {
               ProjectExplorerViewModel?.CreateFolderCommand?.Execute().Subscribe();
           },
           canExecute: isSolutionOpen),
          icon: "add_file_24_rounded".GetIcon(),
          "Ctrl+N"));*/


        return listBuilder;
    }

    private async Task SaveReportData()
    {
        if (ProjectEditingViewModel is { SelectedItem: not null })
        {
            _ = await ProjectEditingViewModel.SelectedItem.SaveItemCommand.Execute();
        }
    }

    private void PrintSolution() =>
        Navigate(NavigationTarget.FullScreen)
            .To(
                ProjectPrintPreviewViewModel,
                NavigationMode.Normal,
                ProjectExplorerViewModel.SolutionState);

    private async Task<Unit> OpenSolution()
    {
        var path = await FileDialogHelper.ShowOpenFileDialogAsync("Abrir Projeto");

        if (path is { Length: > 0 })
        {
            await ReadSolutionAndOpen(path);
        }

        return Unit.Default;
    }

    private async Task ReadSolutionAndOpen(
        string path
    )
    {
        var result = await _mediator.Send(
            new ReadSolutionProjectQuery(path),
            CancellationToken.None);

        result.IfSucc(success =>
        {
            Dispatcher
                .UIThread
                .Post(
                    () =>
                    {
                        ProjectExplorerViewModel = new ProjectExplorerViewModel(
                            success
                        );
                        this.RaisePropertyChanged(nameof(ProjectExplorerViewModel));
                    });
        });
    }

    private async Task CreateSolution()
    {
        var dialogResult = await NavigateDialogAsync(
            new CreateSolutionViewModel("Criar Solução",
                ProjectExplorerViewModel?.SolutionState ?? ProjectSolutionModel.Create("", new SolutionInfo())
            )
            , NavigationTarget.CompactDialogScreen);

        if (dialogResult is { Result: { } dialogData, Kind: DialogResultKind.Normal })
        {
            _ = await _mediator.Send(new CreateSolutionCommand(dialogData.FilePath, dialogData)
                , CancellationToken.None);

            Dispatcher
                .UIThread
                .Post(
                    () =>
                    {
                        ProjectExplorerViewModel = new ProjectExplorerViewModel(
                            dialogData
                        );
                        this.RaisePropertyChanged(nameof(ProjectExplorerViewModel));
                    });
            /* NotificationHelpers.Show(title: "Create", message: "Create Project?", onClick: () =>
             {
                 Logger.LogDebug(message: $"create Project {dialogData.FileName}");
             });*/
        }
    }

    protected override void OnNavigatedTo(
        bool isInHistory
        , CompositeDisposable disposables
        , object? Parameter = null
    )
    {
        if (Parameter is string { Length: > 0 } path)
        {
            Dispatcher.UIThread.Post(
                () =>
                {
                    _ = Task.WhenAll(ReadSolutionAndOpen(path));
                });
        }
    }
}

public static class ProjectInteractions
{
    public static Interaction<Unit, Unit> SyncSolutionInteraction = new();
}