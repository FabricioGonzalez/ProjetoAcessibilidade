using System.Collections.ObjectModel;
using System.Reactive;
using ProjectAvalonia.Presentation.States.ValidationRulesState;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class CheckboxItemState : ReactiveObject
{
    private string _id;

    private ObservableCollection<OptionsItemState> _options;
    private ObservableCollection<TextItemState> _textItems;
    private string _topic;
    private ObservableCollection<ValidationRuleContainerState> _validationRules = new();

    public CheckboxItemState()
    {
        _options = new ObservableCollection<OptionsItemState>();
        _textItems = new ObservableCollection<TextItemState>();
    }

    public ObservableCollection<ValidationRuleContainerState> ValidationRules
    {
        get => _validationRules;
        set => this.RaiseAndSetIfChanged(ref _validationRules, value);
    }

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public ObservableCollection<OptionsItemState> Options
    {
        get => _options;
        set => this.RaiseAndSetIfChanged(ref _options, value);
    }

    public ObservableCollection<TextItemState> TextItems
    {
        get => _textItems;
        set => this.RaiseAndSetIfChanged(ref _textItems, value);
    }

    public ReactiveCommand<Unit, Unit> AddOption => ReactiveCommand.Create(() =>
    {
        Options.Add(new OptionsItemState { Id = Guid.NewGuid().ToString() });
    });

    public ReactiveCommand<Unit, Unit> AddTextItem => ReactiveCommand.Create(() =>
    {
        TextItems.Add(new TextItemState("", "", id: Guid.NewGuid().ToString()));
    });

    public ReactiveCommand<OptionsItemState, Unit> RemoveOption => ReactiveCommand.Create<OptionsItemState>(item =>
    {
        Options.Remove(item);
    });

    public ReactiveCommand<TextItemState, Unit> RemoveTextItem => ReactiveCommand.Create<TextItemState>(item =>
    {
        TextItems.Remove(item);
    });

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(ref _topic, value);
    }
}