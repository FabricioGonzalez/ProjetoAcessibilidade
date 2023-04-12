using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using Common;
using Core.Entities.App;
using Core.Entities.Solution;
using Project.Domain.Contracts;
using Project.Domain.Solution.Commands.SolutionItem;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Common.Mappers;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.PDFViewer.ViewModels;
using ProjectAvalonia.Logging;
using ProjectAvalonia.Stores;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ProjectAvalonia.ViewModels.Navigation;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;

[NavigationMetaData(
    Title = "Projeto",
    Caption = "Criar e editar projetos",
    Order = 0,
    Category = "Projetos",
    Keywords = new[]
    {
        "Projeto"
    },
    NavBarPosition = NavBarPosition.Top,
    NavigationTarget = NavigationTarget.HomeScreen,
    IconName = "edit_file_regular")]
public partial class ProjectViewModel : NavBarItemViewModel
{
    private readonly AddressesStore _addressesStore;
    private readonly SolutionStore _solutionStore;

    private readonly ICommandDispatcher? commandDispatcher;

    private readonly IQueryDispatcher? queryDispatcher;

    public ProjectViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
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

            if (dialogResult is { Result: { } dialogData, Kind: DialogResultKind.Normal })
            {
                NotificationHelpers.Show(title: "Create", message: "Create Project?", onClick: () =>
                {
                    Logger.LogDebug(message: $"create Project {dialogData.FileName}");
                });
            }
        });
        (CreateProjectCommand as ReactiveCommand<Unit, Unit>)
            .IsExecuting
            .ToProperty(source: this,
                property: nameof(IsBusy), result: out _isBusy);

        ((ReactiveCommand<Unit, Unit>)CreateProjectCommand)
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

    public ICommand OpenProjectCommand
    {
        get;
    }

    public ICommand CreateProjectCommand
    {
        get;
    }

    public ICommand PrintProjectCommand
    {
        get;
    }

    public ICommand SaveSolutionCommand
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
            .Select(selector: prop => !string.IsNullOrWhiteSpace(value: prop));
}