using System.Collections.ObjectModel;

using DynamicData.Binding;

using ProjetoAcessibilidade.Core.Enuns;

using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class CheckboxContainerItemState : FormItemStateBase
{
    private ObservableCollection<CheckboxItemState> _children;
    public ObservableCollection<CheckboxItemState> Children
    {
        get => _children;
        set => this.RaiseAndSetIfChanged(ref _children, value);
    }

    private string _topic;
    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(ref _topic, value);
    }


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