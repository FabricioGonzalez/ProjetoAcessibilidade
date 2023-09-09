using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Enums;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class ObservationContainerItemState : FormItemStateBase
{
    private string _id = "";
    private ObservableCollection<ObservationItemState> _observations = new();


    private string _topic = "";

    public ObservationContainerItemState(
        AppFormDataType type = AppFormDataType.Observation
        , string id = ""
    ) : base(type: type, id: id)
    {
    }

    public ObservableCollection<ObservationItemState> Observations
    {
        get => _observations;
        set => this.RaiseAndSetIfChanged(backingField: ref _observations, newValue: value);
    }

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(backingField: ref _id, newValue: value);
    }

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(backingField: ref _topic, newValue: value);
    }
}