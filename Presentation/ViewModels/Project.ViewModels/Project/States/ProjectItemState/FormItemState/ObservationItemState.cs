using ReactiveUI;

namespace AppViewModels.Project.States.ProjectItemState.FormItemState;
public class ObservationItemState : ReactiveObject
{
    private string topic = "";
    public string Topic
    {
        get => topic;
        set => this.RaiseAndSetIfChanged(ref topic, value);
    }

    private string observation = "";
    public string Observation
    {
        get => observation;
        set => this.RaiseAndSetIfChanged(ref observation, value);
    }
}
