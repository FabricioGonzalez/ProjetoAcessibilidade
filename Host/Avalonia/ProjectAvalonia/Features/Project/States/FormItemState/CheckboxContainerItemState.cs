using Core.Enuns;
using DynamicData.Binding;

namespace ProjectAvalonia.Features.Project.States.FormItemState;

public partial class CheckboxContainerItemState : FormItemStateBase
{
    [AutoNotify]
    private ObservableCollectionExtended<CheckboxItemState> children;

    [AutoNotify]
    private string topic;

    public CheckboxContainerItemState(
        string topic
        , AppFormDataType type = AppFormDataType.Checkbox
        , string id = ""
    )
        : base(type: type, id: id)
    {
        Topic = topic;
        Children = new ObservableCollectionExtended<CheckboxItemState>();
    }
}