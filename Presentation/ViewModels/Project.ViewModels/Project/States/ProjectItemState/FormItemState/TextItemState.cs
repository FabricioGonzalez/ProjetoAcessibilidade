using Core.Enuns;

using ReactiveUI;

namespace AppViewModels.Project.States.ProjectItemState.FormItemState;
public class TextItemState : ReactiveObject
{
    private string topic;
    public string Topic
    {
        get => topic;
        set => this.RaiseAndSetIfChanged(ref topic, value);
    }

    private AppFormDataType type;
    public AppFormDataType Type
    {
        get => type;
        set => this.RaiseAndSetIfChanged(ref type, value);
    }

    private string textData = "";
    public string TextData
    {
        get => textData;
        set => this.RaiseAndSetIfChanged(ref textData, value);
    }

    private string measurementUnit = null;
    public string MeasurementUnit
    {
        get => measurementUnit;
        set => this.RaiseAndSetIfChanged(ref measurementUnit, value);
    }

}
