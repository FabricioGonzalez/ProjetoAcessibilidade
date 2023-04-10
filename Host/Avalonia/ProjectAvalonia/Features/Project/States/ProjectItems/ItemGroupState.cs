using System.Collections.ObjectModel;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.ProjectItems;

public partial class ItemGroupState : ReactiveObject
{
    [AutoNotify]
    private bool _inEditMode;

    [AutoNotify]
    private string _itemPath = "";

    [AutoNotify]
    private ObservableCollection<ItemState> _items = new();

    [AutoNotify]
    private string _name = "";
}