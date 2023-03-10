using System.Collections.ObjectModel;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.ProjectItems;
public partial class ItemGroupState : ReactiveObject
{
    [AutoNotify]
    private string _name = "";

    [AutoNotify]
    private bool _inEditMode = false;
    [AutoNotify]
    private ObservableCollection<ItemState> _items = new();
}
