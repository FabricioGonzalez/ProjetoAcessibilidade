using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using AppViewModels.Common;
using AppViewModels.Dialogs;
using AppViewModels.Dialogs.States;
using AppViewModels.Interactions.Project;
using AppViewModels.Project.ComposableViewModels;
using AppViewModels.Project.Mappers;
using AppViewModels.Project.Operations;
using AppViewModels.Project.States;

using Common;

using Core.Entities.Solution;
using Core.Entities.Solution.Explorer;

using Project.Application.Contracts;
using Project.Application.Project.Queries.GetProjectItems;
using Project.Application.Solution.Queries;

using ReactiveUI;

using Splat;

namespace AppViewModels.Project;
public class ProjectExplorerViewModel : ViewModelBase
{
    private string currentOpenProject = null;
    public string CurrentOpenProject
    {
        get => currentOpenProject;
        set => this.RaiseAndSetIfChanged(ref currentOpenProject, value);
    }

    private bool isDocumentSolutionEnabled = false;
    public bool IsDocumentSolutionEnabled
    {
        get => isDocumentSolutionEnabled;
        set => this.RaiseAndSetIfChanged(ref isDocumentSolutionEnabled, value, nameof(IsDocumentSolutionEnabled));
    }

    public ProjectExplorerState projectExplorerState
    {
        get; set;
    }
    public SolutionStateViewModel SolutionModel
    {
        get;
    }

    public readonly ProjectExplorerOperations explorerOperations;
    private readonly IQueryDispatcher queryDispatcher;

    public IObservable<bool> IsProjectOpened;


    public ProjectExplorerViewModel()
    {
        explorerOperations ??= Locator.Current.GetService<ProjectExplorerOperations>();
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();

        SolutionModel ??= Locator.Current.GetService<SolutionStateViewModel>();

        projectExplorerState = new();

        ShowDialog = new();

        AddItemCommand = ReactiveCommand.CreateFromTask<ProjectItemViewModel>(async (item) =>
        {
            var store = new AddItemViewModel();

            var result = await ShowDialog.Handle(store);

            if (result is not null)
            {
                ((FolderProjectItemViewModel)item)
                .Children
                .Add(
                    new FileProjectItemViewModel(
                        title: result.Name,
                        path: Path.Combine(item.Path, $"{result.Name}{Constants.AppProjectItemExtension}"),
                        referencedItem: result.Path,
                        inEditMode: true)
                    );
            }
        }, IsProjectOpened);

        AddFolderCommand = ReactiveCommand.Create<ProjectItemViewModel>((item) =>
        {
            if (item is not null)
            {
                ((FolderProjectItemViewModel)item)
                .Children
                .Add(
                    new FolderProjectItemViewModel(
                    title: "",
                    path: item.Path,
                    inEditMode: true,
                    referencedItem: "")
                    );
            }
        }, IsProjectOpened);

        SelectSolutionItemCommand = ReactiveCommand.Create<ProjectItemViewModel>((item) =>
        {
            ProjectEditingInteractions
            .EditItem
            .Handle((item as FileProjectItemViewModel))
            .Subscribe();
        }, IsProjectOpened);

        this.WhenActivated(disposables =>
        {
            IsProjectOpened = this.WhenAnyValue(x => x.CurrentOpenProject,
             prop => !string.IsNullOrWhiteSpace(prop));

            SolutionModel.Activator.Activate();

            this
             .WhenAnyValue(vm => vm.CurrentOpenProject)
             .Where(value => !string.IsNullOrEmpty(value))
             .Subscribe(async path =>
             {
                 if (path.Length > 0)
                 {
                     var result = await queryDispatcher
                     .Dispatch<GetProjectItemsQuery, Resource<List<ExplorerItem>>>(new(path), CancellationToken.None);
                     if (result is Resource<List<ExplorerItem>>.Success data)
                     {
                         var items = data.Data.GetSubfolders();

                         if (items is not null && items.Count > 0)
                         {
                             projectExplorerState.ExplorerItems = new(items);
                         }
                     }

                 }
             })
             .DisposeWith(disposables);

            SolutionModel.ChooseSolutionPath
            .Subscribe(result =>
            {
                SolutionModel.FilePath = result;
            })
           .DisposeWith(disposables);

            SolutionModel.ChooseLogoPath
            .Subscribe(result =>
            {
                SolutionModel.ReportData.LogoPath = result;
            })
            .DisposeWith(disposables);
        });
    }

    public async Task<ProjectSolutionModel>? ReadSolution(string path)
    {
        (await queryDispatcher.Dispatch<ReadSolutionProjectQuery, Resource<ProjectSolutionModel>>(new(path), CancellationToken.None))
               .OnError(out var data, out var message)
               .OnLoading(out data, out var isLoading)
               .OnSuccess(out data);

        if (data is not null)
        {
            return data;
        }

        return new();

    }
    public ReactiveCommand<string, Unit> PrintDocument
    {
        get; set;
    }
    public ReactiveCommand<ProjectItemViewModel, Unit> AddItemCommand
    {
        get;
    }
    public ReactiveCommand<ProjectItemViewModel, Unit> AddFolderCommand
    {
        get;
    }
    public ReactiveCommand<ProjectItemViewModel, Unit> SelectSolutionItemCommand
    {
        get;
    }
    public Interaction<AddItemViewModel, ExplorerItem?> ShowDialog
    {
        get; private set;
    }
}
