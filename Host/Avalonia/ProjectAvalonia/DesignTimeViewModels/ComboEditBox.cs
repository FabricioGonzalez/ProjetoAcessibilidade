using ProjectAvalonia.Presentation.Enums;
using ProjectAvalonia.Presentation.States.FormItemState;

namespace ProjectAvalonia.DesignTimeViewModels;

public class ComboEditBox : FormItemContainer
{
    public string Id
    {
        get;
        set;
    }

    public AppFormDataType Type
    {
        get;
        set;
    } = AppFormDataType.Checkbox;

    public string Topic
    {
        get;
        set;
    }

    public FormItemStateBase Body
    {
        get;
        set;
    } = new TextItemState(
        topic: "teste item",
        id: "",
        textData: "teste",
        measurementUnit: "m");
}