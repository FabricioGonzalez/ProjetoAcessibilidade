using AppViewModels.Project.ComposableViewModels;

using Core.Entities.Solution.Explorer;

using DynamicData;
using DynamicData.Binding;

using ReactiveUI;

namespace AppViewModels.Project.States;
public class ProjectExplorerState : ReactiveObject
{
    private SourceList<FolderItem> _items;

    private ObservableCollectionExtended<ProjectItemViewModel> explorerItems = new();
    public ObservableCollectionExtended<ProjectItemViewModel> ExplorerItems
    {
        get => explorerItems;
        set => this.RaiseAndSetIfChanged(ref explorerItems, value, nameof(ExplorerItems));
    }
}
