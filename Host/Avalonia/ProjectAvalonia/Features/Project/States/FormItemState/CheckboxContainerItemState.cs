using Core.Enuns;

using DynamicData.Binding;

namespace ProjectAvalonia.Features.Project.States.FormItemState;
public partial class CheckboxContainerItemState : FormItemStateBase
{
    [AutoNotify]
    private string topic;
    [AutoNotify]
    private ObservableCollectionExtended<CheckboxItemState> children;

    public CheckboxContainerItemState(string topic,
        AppFormDataType type = AppFormDataType.Checkbox,
        string id = "")
        : base(type, id)
    {
        Topic = topic;
        Children = new();
    }

}
