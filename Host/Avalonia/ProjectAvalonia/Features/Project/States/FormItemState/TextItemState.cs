using Core.Enuns;

namespace ProjectAvalonia.Features.Project.States.FormItemState;

public partial class TextItemState : FormItemStateBase
{
    [AutoNotify]
    private string? _measurementUnit;

    [AutoNotify]
    private string _textData = "";

    [AutoNotify]
    private string _topic = "";

    public TextItemState(
        string topic
        , string textData
        , string? measurementUnit = null
        , AppFormDataType type = AppFormDataType.Texto
        , string id = ""
    )
        : base(type: type, id: id)
    {
        Topic = topic;
        TextData = textData;
        MeasurementUnit = measurementUnit;
    }
}