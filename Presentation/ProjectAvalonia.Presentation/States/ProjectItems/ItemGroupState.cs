using System.Collections.ObjectModel;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.ProjectItems;

public class ItemGroupState : ReactiveObject
{
    private ObservableCollection<ItemState> _items;
    private string _name = "";

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(backingField: ref _name, newValue: value);
    }

    public ObservableCollection<ItemState> Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(backingField: ref _items, newValue: value);
    }
}