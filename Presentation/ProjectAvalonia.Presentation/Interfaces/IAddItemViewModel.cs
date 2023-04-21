using System.Reactive;
using ProjectAvalonia.Presentation.States.ProjectItems;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IAddItemViewModel
{
    ItemState? SelectedItem
    {
        get;
        set;
    }

    IEnumerable<ItemState>? Items
    {
        get;
    }

    public string ItemName
    {
        get;
        set;
    }

    public ReactiveCommand<Unit, Unit> LoadAllItems
    {
        get;
    }
}