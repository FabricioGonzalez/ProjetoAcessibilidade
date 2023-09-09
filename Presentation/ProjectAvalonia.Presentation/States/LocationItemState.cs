using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.States.ProjectItems;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public class LocationItemState : ReactiveObject
{
    private ObservableCollection<ItemGroupState> _itemGroups = new();
    private string _name = "";

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(backingField: ref _name, newValue: value);
    }

    public ObservableCollection<ItemGroupState> ItemGroup
    {
        get => _itemGroups;
        set => this.RaiseAndSetIfChanged(backingField: ref _itemGroups, newValue: value);
    }
}