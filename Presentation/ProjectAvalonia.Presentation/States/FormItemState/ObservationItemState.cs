using ProjectAvalonia.Presentation.Enums;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class ObservationItemState : FormItemStateBase
{
    private string _observation = "";


    private string _topic = "";

    public ObservationItemState(
        string topic
        , string observation
        , AppFormDataType type
        , string id = ""
    )
        : base(type: type, id: id)
    {
        Observation = observation;
        Topic = topic;
    }

    public string Observation
    {
        get => _observation;
        set => this.RaiseAndSetIfChanged(backingField: ref _observation, newValue: value);
    }

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(backingField: ref _topic, newValue: value);
    }
}