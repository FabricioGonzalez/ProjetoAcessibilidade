using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.States.ProjectItems;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public class SolutionState : ReactiveObject
{
    private string _fileName = "";
    private string _filePath = "";

    private ObservableCollection<LocationItemState> _locationItems;

    private string _logoPath = "";

    private SolutionReportState _report;

    public SolutionReportState Report
    {
        get => _report;
        set => this.RaiseAndSetIfChanged(backingField: ref _report, newValue: value);
    }

    public string LogoPath
    {
        get => _logoPath;
        set => this.RaiseAndSetIfChanged(backingField: ref _logoPath, newValue: value);
    }

    public string FilePath
    {
        get => _filePath;
        set => this.RaiseAndSetIfChanged(backingField: ref _filePath, newValue: value);
    }

    public string FileName
    {
        get => _fileName;
        set => this.RaiseAndSetIfChanged(backingField: ref _fileName, newValue: value);
    }

    public ObservableCollection<LocationItemState> LocationItems
    {
        get => _locationItems;
        set => this.RaiseAndSetIfChanged(backingField: ref _locationItems, newValue: value);
    }
}

public class LocationItemState : ReactiveObject
{
    private ObservableCollection<ItemGroupState> _itemGroups;
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