using Core.Enuns;

using DynamicData.Binding;

using ReactiveUI;

namespace AppViewModels.Project.States.ProjectItemState.FormItemState;
public class CheckboxContainerItemState : ReactiveObject
{
    private string topic;
    public string Topic
    {
        get => topic;
        set => this.RaiseAndSetIfChanged(ref topic, value);
    }

    private AppFormDataType type = AppFormDataType.Checkbox;
    public AppFormDataType Type
    {
        get => type;
        set => this.RaiseAndSetIfChanged(ref type, value);
    }

    private ObservableCollectionExtended<CheckboxItemState> children;
    public ObservableCollectionExtended<CheckboxItemState> Children
    {
        get => children;
        set => this.RaiseAndSetIfChanged(ref children, value);
    }

}
