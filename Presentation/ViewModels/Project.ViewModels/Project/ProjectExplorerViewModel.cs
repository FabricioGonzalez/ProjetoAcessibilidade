using System.Reactive.Disposables;

using AppUsecases.Editing.Entities;
using AppUsecases.Project.Entities.FileTemplate;

using AppViewModels.Common;
using AppViewModels.Dialogs;

using DynamicData.Binding;

using DynamicData;

using ReactiveUI;

using AppViewModels.Project.ComposableViewModels;
using AppViewModels.Project.States;
using MediatR;
using Project.Application.Project.Queries.GetProjectItems;
using Splat;
using AppViewModels.Project.Mappers;
using AppViewModels.Project.Operations;

namespace AppViewModels.Project;
public class ProjectExplorerViewModel : ViewModelBase
{
    private string currentOpenProject;
    public string CurrentOpenProject
    {
        get => currentOpenProject;
        set => this.RaiseAndSetIfChanged(ref currentOpenProject, value, nameof(CurrentOpenProject));
    }

    public ProjectExplorerState projectExplorerState
    {
        get; set;
    }

    public readonly ProjectExplorerOperations explorerOperations;

    readonly GetProjectItemsQueryHandler getProjectItems;

    public ProjectExplorerViewModel()
    {
        getProjectItems ??= Locator.Current.GetService<GetProjectItemsQueryHandler>();
        explorerOperations ??= Locator.Current.GetService<ProjectExplorerOperations>();

        projectExplorerState = new();

        this.WhenActivated(disposables =>
        {
            this
             .WhenAnyValue(vm => vm.CurrentOpenProject)
             .WhereNotNull()
             .Subscribe(async path =>
             {
                 if(path.Length > 0)
                 {
                     var result = await getProjectItems.Handle(new(path), CancellationToken.None);

                     var items = result.GetSubfolders();

                     if(items is not null && items.Count > 0)
                     {
                         projectExplorerState.ExplorerItems = new(items);
                     }

                 }
             }).DisposeWith(disposables);
        });
    }


    public Interaction<AddItemViewModel, FileTemplate?> ShowDialog
    {
        get; private set;
    }
}
