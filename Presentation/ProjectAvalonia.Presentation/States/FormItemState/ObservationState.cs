using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class ObservationState : ReactiveObject
{
    private string _observation = "";
    private string _topic = "";

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(backingField: ref _topic, newValue: value);
    }

    public string Observation
    {
        get => _observation;
        set => this.RaiseAndSetIfChanged(backingField: ref _observation, newValue: value);
    }
}