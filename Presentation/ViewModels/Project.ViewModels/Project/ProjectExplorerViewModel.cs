using System.Reactive.Disposables;

using AppUsecases.Editing.Entities;

using AppViewModels.Common;
using AppViewModels.Dialogs;
using AppViewModels.Project.States;

using ReactiveUI;

using Project.Application.Project.Queries.GetProjectItems;

using Splat;

using AppViewModels.Project.Mappers;
using AppViewModels.Project.Operations;
using AppViewModels.Dialogs.States;
using AppUsecases.Contracts.Usecases;
using AppUsecases.Project.Entities.Project;
using Common;
using System.Reactive.Linq;
using AppViewModels.Project.ComposableViewModels;
using System.Reactive;

namespace AppViewModels.Project;
public class ProjectExplorerViewModel : ViewModelBase
{
    private string currentOpenProject;
    public string CurrentOpenProject
    {
        get => currentOpenProject;
        set => this.RaiseAndSetIfChanged(ref currentOpenProject, value, nameof(CurrentOpenProject));
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

    private readonly IQueryUsecase<string, ProjectSolutionModel> readSolution;
    public readonly ProjectExplorerOperations explorerOperations;

    readonly GetProjectItemsQueryHandler getProjectItems;

    public ProjectExplorerViewModel()
    {
        getProjectItems ??= Locator.Current.GetService<GetProjectItemsQueryHandler>();
        explorerOperations ??= Locator.Current.GetService<ProjectExplorerOperations>();

        SolutionModel = Locator.Current.GetService<SolutionStateViewModel>();
        readSolution = Locator.Current.GetService<IQueryUsecase<string, ProjectSolutionModel>>();

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
                        path: Path.Combine(item.Path,$"{result.Name}{Constants.AppProjectItemExtension}"),
                        referencedItem: result.FilePath,
                        inEditMode: true)
                    );
            }
        });

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
                    referencedItem:"")
                    );
            }
        });

        this.WhenActivated(disposables =>
        {
            this
             .WhenAnyValue(vm => vm.CurrentOpenProject)
             .WhereNotNull()
             .Subscribe(async path =>
             {
                 if (path.Length > 0)
                 {
                     var result = await getProjectItems.Handle(new(path), CancellationToken.None);

                     var items = result.GetSubfolders();

                     if (items is not null && items.Count > 0)
                     {
                         projectExplorerState.ExplorerItems = new(items);
                     }

                 }
             }).DisposeWith(disposables);

            SolutionModel.ChooseSolutionPath.Subscribe(result =>
            {
                SolutionModel.FilePath = result;
            })
           .DisposeWith(disposables);

            SolutionModel.ChooseLogoPath.Subscribe(result =>
            {
                SolutionModel.ReportData.LogoPath = result;
            }).DisposeWith(disposables);
        });
    }

    public async Task<ProjectSolutionModel>? ReadSolution(string path)
    {
        (await readSolution.executeAsync(path))
               .OnError(out var data, out var message)
               .OnLoading(out data, out var isLoading)
               .OnSuccess(out data);

        if (data is not null)
        {
            return data;
        }

        return null;

    }

    public ReactiveCommand<ProjectItemViewModel, Unit> AddItemCommand
    {
        get;
    }
    public ReactiveCommand<ProjectItemViewModel, Unit> AddFolderCommand
    {
        get;
    }

    public Interaction<AddItemViewModel, FileTemplate?> ShowDialog
    {
        get; private set;
    }
}
