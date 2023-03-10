using System.Collections.ObjectModel;

using Core.Entities.Solution.ItemsGroup;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.ProjectItems;
public partial class ItemState : ReactiveObject
{

    [AutoNotify]
    private string _id = "";

    [AutoNotify]
    private string _name = "";

    [AutoNotify]
    private string _itemPath = "";

    [AutoNotify]
    private string _templateName = "";

    [AutoNotify]
    private bool _inEditMode = false;

    [AutoNotify]
    private ObservableCollection<ItemModel> _items = new();

}
