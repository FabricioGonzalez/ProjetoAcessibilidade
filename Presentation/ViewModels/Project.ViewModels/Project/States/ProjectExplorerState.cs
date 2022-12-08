using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppUsecases.Project.Entities.FileTemplate;
using DynamicData.Binding;
using DynamicData;
using AppViewModels.Project.ComposableViewModels;
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
