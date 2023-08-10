using System.Collections.ObjectModel;
using System.Reactive;
using DynamicData.Binding;
using ProjectAvalonia.Presentation.Enums;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class CheckboxContainerItemState
    : FormItemStateBase
        , IIdentifiedItem
{
    private ObservableCollection<CheckboxItemState> _children;

    private string _topic;

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

    public ObservableCollection<CheckboxItemState> Children
    {
        get => _children;
        set => this.RaiseAndSetIfChanged(backingField: ref _children, newValue: value);
    }

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(backingField: ref _topic, newValue: value);
    }

    public ReactiveCommand<Unit, Unit> AddCheckboxItem => ReactiveCommand.Create(() =>
    {
        Children.Add(new CheckboxItemState { Id = Guid.NewGuid().ToString() });
    });

    public ReactiveCommand<CheckboxItemState, Unit> AddOption => ReactiveCommand.Create<CheckboxItemState>(item =>
    {
        item.Options.Add(new OptionsItemState { Id = Guid.NewGuid().ToString() });
    });

    public ReactiveCommand<CheckboxItemState, Unit> AddTextItem => ReactiveCommand.Create<CheckboxItemState>(item =>
    {
        item.TextItems.Add(new TextItemState(topic: "", textData: "", id: Guid.NewGuid().ToString()));
    });

    public ReactiveCommand<CheckboxItemState, Unit> RemoveCheckboxItem => ReactiveCommand.Create<CheckboxItemState>(
        item =>
        {
            Children.Remove(item);
        });
}