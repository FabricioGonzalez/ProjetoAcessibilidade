using System;
using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Avalonia.Threading;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.PDFViewer.ViewModels;
using ProjectAvalonia.Models;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;

using ProjetoAcessibilidade.Core.Entities.Solution;
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
    : NavBarItemViewModel,
        IProjectViewModel
{
    private readonly ObservableAsPropertyHelper<bool> _isSolutionOpen;

    private readonly IMediator _mediator;

    private ProjectSolutionModel? projectSolution;

    public ProjectViewModel()
    {
        SetupCancel(
            enableCancel: false,
            enableCancelOnEscape: true,
            enableCancelOnPressed: true);

        var isSolutionOpen = this.WhenAnyValue(property1: vm => vm.IsSolutionOpen);

        this.WhenAnyValue(property1: vm => vm.ProjectExplorerViewModel)
           .Select(selector: x => (x?.Items.Count > 0) || (x?.SolutionState is not null))
           .ToProperty(
               source: this,
               property: x => x.IsSolutionOpen,
               result: out _isSolutionOpen);

        ToolBar = new MenuViewModel(CreateMenu(isSolutionOpen).ToImmutable());


        ProjectInteractions.SyncSolutionInteraction.RegisterHandler(
            context =>
            {
                SaveSolutionCommand?.Execute();

                context.SetOutput(Unit.Default);
            });

        SelectionMode = NavBarItemSelectionMode.Button;

        _mediator ??= Locator.Current.GetService<IMediator>();

        OpenProjectCommand = ReactiveCommand.CreateFromTask(execute: OpenSolution);

        CreateProjectCommand = ReactiveCommand.CreateFromTask(execute: CreateSolution);

        PrintProjectCommand = ReactiveCommand.Create(
            execute: PrintSolution,
            canExecute: isSolutionOpen);

        ProjectEditingViewModel = new ProjectEditingViewModel();
        ProjectPrintPreviewViewModel = new PreviewerViewModel();

        EnableAutoBusyOn(
            OpenProjectCommand,
            CreateProjectCommand);
    }

    private ImmutableList<IMenuItem>.Builder CreateMenu(IObservable<bool> isSolutionOpen)
    {
        var listBuilder = ImmutableList.CreateBuilder<IMenuItem>();

        listBuilder.Add(new MenuItemModel(
            label: "Open",
            command: ReactiveCommand.CreateFromTask(execute: OpenSolution),
            icon: "file_open_24_rounded".GetIcon(),
            "Ctrl+Shift+O"));

        listBuilder.Add(new MenuItemModel(
            label: "Create Solution",
            command: ReactiveCommand.CreateFromTask(execute: CreateSolution),
            icon: "solution_create_24_rounded".GetIcon(),
            "Ctrl+Shift+N"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
           label: "Save Current Item",
           command: ReactiveCommand.CreateFromTask(
            execute: SaveReportData,
            canExecute: isSolutionOpen),
           icon: "save_data_24_rounded".GetIcon(),
           "Ctrl+S"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
          label: "Print Solution",
          command: ReactiveCommand.Create(
           execute: PrintSolution,
           canExecute: isSolutionOpen),
          icon: "print_24_rounded".GetIcon(),
          "Ctrl+P"));

        listBuilder.Add(new MenuItemSeparatorModel());

        listBuilder.Add(new MenuItemModel(
         label: "Add Folder",
         command: ReactiveCommand.Create(
          execute: () =>
          {
              ProjectExplorerViewModel?.CreateFolderCommand?.Execute().Subscribe();
          },
          canExecute: isSolutionOpen),
         icon: "add_folder_24_rounded".GetIcon(),
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

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;

    public PreviewerViewModel ProjectPrintPreviewViewModel
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

    public override MenuViewModel? ToolBar
    {
        get;
    }

    private async Task SaveReportData()
    {
        if (ProjectEditingViewModel is { SelectedItem: { } })
        {
            await ProjectEditingViewModel.SelectedItem.SaveItemCommand.Execute();
        }
    }

    private void PrintSolution() =>
        Navigate(currentTarget: NavigationTarget.FullScreen)
            .To(
                viewmodel: ProjectPrintPreviewViewModel,
                mode: NavigationMode.Normal,
                Parameter: ProjectExplorerViewModel.SolutionState);

    private async Task<Unit> OpenSolution()
    {
        var path = await FileDialogHelper.ShowOpenFileDialogAsync(title: "Abrir Projeto");

        if (path is { Length: > 0 })
        {
            await ReadSolutionAndOpen(path: path);
        }

        return Unit.Default;
    }

    private async Task ReadSolutionAndOpen(
        string path
    )
    {
        var result = await _mediator.Send(
            request: new ReadSolutionProjectQuery(SolutionPath: path),
            cancellation: CancellationToken.None);

        result.IfSucc(success =>
        {
            Dispatcher
            .UIThread
            .Post(
             action: () =>
            {
                ProjectExplorerViewModel = new ProjectExplorerViewModel(
                    state: success
                );
                this.RaisePropertyChanged(propertyName: nameof(ProjectExplorerViewModel));
            });
        });

    }
    private async Task CreateSolution()
    {
        var dialogResult = await NavigateDialogAsync(
                dialog: new CreateSolutionViewModel(title: "Criar Solução",
                ProjectExplorerViewModel?.SolutionState ?? ProjectSolutionModel.Create("", new())
                )
                , target: NavigationTarget.CompactDialogScreen);

        if (dialogResult is { Result: { } dialogData, Kind: DialogResultKind.Normal })
        {

            await _mediator.Send(new CreateSolutionCommand(dialogData.FilePath, dialogData), CancellationToken.None);

            Dispatcher
                  .UIThread
                  .Post(
                      action: () =>
                      {
                          ProjectExplorerViewModel = new ProjectExplorerViewModel(
                              state: dialogData
                          );
                          this.RaisePropertyChanged(propertyName: nameof(ProjectExplorerViewModel));
                      });
            /* NotificationHelpers.Show(title: "Create", message: "Create Project?", onClick: () =>
             {
                 Logger.LogDebug(message: $"create Project {dialogData.FileName}");
             });*/
        }

    }

    protected override void OnNavigatedTo(
        bool isInHistory,
        CompositeDisposable disposables,
        object? Parameter = null
    )
    {
        if (Parameter is string { Length: > 0 } path)
        {
            Dispatcher.UIThread.Post(
                action: () =>
                {
                    Task.WhenAll(ReadSolutionAndOpen(path: path));
                });
        }
    }
}

public static class ProjectInteractions
{
    public static Interaction<Unit, Unit> SyncSolutionInteraction = new();
}