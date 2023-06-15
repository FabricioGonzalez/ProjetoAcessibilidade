using System.Collections.ObjectModel;
using System.Reactive;

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

    public ReactiveCommand<Unit, Unit> AddCheckboxItem => ReactiveCommand.Create(() =>
    {
        Children.Add(new() { Id = Guid.NewGuid().ToString() });
    });

    public ReactiveCommand<CheckboxItemState, Unit> AddOption => ReactiveCommand.Create<CheckboxItemState>((item) =>
    {
        item.Options.Add(new OptionsItemState() { Id = Guid.NewGuid().ToString() });
    });

    public ReactiveCommand<CheckboxItemState, Unit> AddTextItem => ReactiveCommand.Create<CheckboxItemState>((item) =>
    {
        item.TextItems.Add(new TextItemState("", "", id: Guid.NewGuid().ToString()));
    });

    public ReactiveCommand<CheckboxItemState, Unit> RemoveCheckboxItem => ReactiveCommand.Create<CheckboxItemState>((item) =>
    {
        Children.Remove(item);
    });
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