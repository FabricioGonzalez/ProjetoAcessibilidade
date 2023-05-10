using ProjetoAcessibilidade.Core.Enuns;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class TextItemState : FormItemStateBase
{
    private string? _measurementUnit;

    private string _textData = "";

    private string _topic = "";

    public TextItemState(
        string topic
        , string textData
        , string? measurementUnit = null
        , AppFormDataType type = AppFormDataType.Texto
        , string id = ""
    )
        : base(type, id)
    {
        Topic = topic;
        TextData = textData;
        MeasurementUnit = measurementUnit;
    }

    public string? MeasurementUnit
    {
        get => _measurementUnit;
        set => this.RaiseAndSetIfChanged(ref _measurementUnit, value);
    }

    public string TextData
    {
        get => _textData;
        set => this.RaiseAndSetIfChanged(ref _textData, value);
    }

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(ref _topic, value);
    }
}