using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Common;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.PDFViewer.ViewModels;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.ViewModels.Navigation;
using ProjetoAcessibilidade.Domain.Contracts;
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

    private readonly IMediator? _queryDispatcher;
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
        SetupCancel(false, true, true);

        SelectionMode = NavBarItemSelectionMode.Button;
        _queryDispatcher ??= Locator.Current.GetService<IMediator>();

        this.WhenAnyValue(vm => vm.ProjectExplorerViewModel)
            .Select(x => x?.Items.Count > 0)
            .ToProperty(this, x => x.IsSolutionOpen, out _isSolutionOpen);

        var isSolutionOpen = this.WhenAnyValue(vm => vm.IsSolutionOpen);

        OpenProjectCommand = ReactiveCommand.CreateFromTask(OpenSolution);
        CreateProjectCommand = ReactiveCommand.CreateFromTask(CreateSolution);
        PrintProjectCommand = ReactiveCommand.CreateFromTask(PrintSolution, isSolutionOpen);

        ProjectEditingViewModel = new ProjectEditingViewModel();
        ProjectPrintPreviewViewModel = new PreviewerViewModel();

        EnableAutoBusyOn(OpenProjectCommand, CreateProjectCommand);
    }

    public PreviewerViewModel ProjectPrintPreviewViewModel
    {
        get;
    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
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

    public ReactiveCommand<SolutionState, Unit> SaveSolutionCommand
    {
        get;
    }

    private async Task<Unit> PrintSolution(
        CancellationToken arg
    )
    {
        Navigate(NavigationTarget.FullScreen)
            .To(ProjectPrintPreviewViewModel,
                NavigationMode.Normal,
                ProjectExplorerViewModel.SolutionState);

        return Unit.Default;
    }

    private async Task<Unit> OpenSolution()
    {
        var path = await FileDialogHelper.ShowOpenFileDialogAsync("Abrir Projeto");

        if (path is not null)
        {
            await ReadSolutionAndOpen(path);
        }

        return Unit.Default;
    }

    private async Task ReadSolutionAndOpen(
        string path
    ) =>
        (await _queryDispatcher.Dispatch(
            new ReadSolutionProjectQuery(path),
            CancellationToken.None))
        .OnSuccess(
            result =>
            {
                Dispatcher
                    .UIThread
                    .Post(() =>
                    {
                        ProjectExplorerViewModel = new ProjectExplorerViewModel(result
                                .Data
                                .ItemGroups
                                .Select(model =>
                                    {
                                        var vm = new ItemGroupViewModel(model.Name,
                                            model.ItemPath);
                                        vm.TransformFrom(model.Items);

                                        return vm;
                                    }
                                ).ToList<IItemGroupViewModel>(),
                            result.Data
                        );
                        this.RaisePropertyChanged(nameof(ProjectExplorerViewModel));
                    });
            })
        .OnError(error =>
        {
        });

    private async Task<Unit> CreateSolution() => Unit.Default;

    protected override void OnNavigatedTo(
        bool isInHistory
        , CompositeDisposable disposables
        , object? Parameter = null
    )
    {
        if (Parameter is string path && !string.IsNullOrWhiteSpace(path))
        {
            Dispatcher.UIThread.Post(() =>
            {
                Task.WhenAll(ReadSolutionAndOpen(path));
            });
        }
    }
}