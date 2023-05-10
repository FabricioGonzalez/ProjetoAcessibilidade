using ProjetoAcessibilidade.Core.Enuns;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class ObservationItemState : FormItemStateBase
{
    private string _observation = "";


    private string _topic = "";

    public ObservationItemState(
        string topic
        , string observation
        , AppFormDataType type = AppFormDataType.Observação
        , string id = ""
    )
        : base(type, id)
    {
        Observation = observation;
        Topic = topic;
    }

    public string Observation
    {
        get => _observation;
        set => this.RaiseAndSetIfChanged(ref _observation, value);
    }

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(ref _topic, value);
    }
}