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

    private readonly IMediator mediator;

    public ProjectExplorerViewModel()
    {
        mediator = Locator.Current.GetService<IMediator>();
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
                     var result = await mediator.Send(new GetProjectItemsQuery(path),CancellationToken.None);

                 }
             }).DisposeWith(disposables);
        });
    }


    public Interaction<AddItemViewModel, FileTemplate?> ShowDialog
    {
        get; private set;
    }
}
