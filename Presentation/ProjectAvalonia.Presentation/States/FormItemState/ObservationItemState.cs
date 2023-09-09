using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class ObservationItemState : ReactiveObject
{
    private string _id = "";
    private string _observation = "";


    private string _topic = "";

    public ObservationItemState(
        string topic
        , string observation
    )
    {
        Observation = observation;
        Topic = topic;
    }

    public string Observation
    {
        get => _observation;
        set => this.RaiseAndSetIfChanged(backingField: ref _observation, newValue: value);
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