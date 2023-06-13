using System;
using System.Threading;
using System.Threading.Tasks;

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

    /*private readonly AddressesStore _addressesStore;
    private readonly SolutionStore _solutionStore;

    private readonly ICommandDispatcher? commandDispatcher;



    public ProjectViewModel()
    {

        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();
        _solutionStore ??= Locator.Current.GetService<SolutionStore>();
        _addressesStore ??= Locator.Current.GetService<AddressesStore>();

        SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);

        SelectionMode = NavBarItemSelectionMode.Button;

        projectExplorerViewModel = new ProjectExplorerViewModel();
        projectEditingViewModel = new ProjectEditingViewModel();

        PrintProjectCommand = ReactiveCommand.Create(execute: () =>
        {
            Navigate(currentTarget: NavigationTarget.FullScreen)
                .To(viewmodel: printPreviewViewModel,
                    mode: NavigationMode.Normal,
                    Parameter: _solutionStore.CurrentOpenSolution);
        }, canExecute: IsSolutionOpened());

        SaveSolutionCommand = ReactiveCommand.CreateFromTask<SolutionState>(
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
                await _solutionStore.LoadSolution(solutionPath: path);
            }
        });

        OpenProjectCommand
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

            if (dialogResult is { Result: { } dialogData, Kind: DialogResultKind.Normal })
            {
                NotificationHelpers.Show(title: "Create", message: "Create Project?", onClick: () =>
                {
                    Logger.LogDebug(message: $"create Project {dialogData.FileName}");
                });
            }
        });
        CreateProjectCommand
            .IsExecuting
            .ToProperty(source: this,
                property: nameof(IsBusy), result: out _isBusy);

        CreateProjectCommand
            .ThrownExceptions
            .Subscribe(onNext: exception =>
            {
                Logger.LogError(message: "Error!", exception: exception);
            });
        Dispatcher.UIThread.Post(action: () =>
        {
            Task.WhenAll(_addressesStore.LoadAllUf(token: GetCancellationToken()));
        });
    }

    public ReadOnlyObservableCollection<UFModel> UfList => _addressesStore?.UfList;

    public SolutionState SolutionState => _solutionStore.CurrentOpenSolution;

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

    public ReactiveCommand<SolutionState, Unit> SaveSolutionCommand
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
            Dispatcher.UIThread.Post(action: async () =>
            {
                await _solutionStore.LoadSolution(solutionPath: path);
            });
        }
    }

    private IObservable<bool> IsSolutionOpened() =>
        this.WhenAnyValue(property1: vm => vm._solutionStore.CurrentOpenSolution.FilePath)
            .Select(selector: prop => !string.IsNullOrWhiteSpace(value: prop));*/

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
         icon: "print_24_rounded".GetIcon(),
         "Ctrl+P"));

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

        if (path is not null)
        {
            await ReadSolutionAndOpen(path: path);
        }

        return Unit.Default;
    }

    private async Task ReadSolutionAndOpen(
        string path
    ) =>
        (await _mediator.Send(
            request: new ReadSolutionProjectQuery(SolutionPath: path),
            cancellation: CancellationToken.None))
        .OnSuccess(
            onSuccessAction: result =>
            {
                projectSolution = result.Data;

                Dispatcher
                .UIThread
                .Post(
                    action: () =>
                    {
                        ProjectExplorerViewModel = new ProjectExplorerViewModel(
                            state: result.Data
                        );
                        this.RaisePropertyChanged(propertyName: nameof(ProjectExplorerViewModel));
                    });
            })
        .OnError(
            onErrorAction: error =>
            {
            });

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
        if (Parameter is string path &&
            !string.IsNullOrWhiteSpace(value: path))
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