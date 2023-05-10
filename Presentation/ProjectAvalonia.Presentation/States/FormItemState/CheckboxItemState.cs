using System.Collections.ObjectModel;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class CheckboxItemState : ReactiveObject
{
    private string _id;

    private ObservableCollection<OptionsItemState> _options;
    private ObservableCollection<TextItemState> _textItems;
    private string _topic;

    public CheckboxItemState()
    {
        _options = new ObservableCollection<OptionsItemState>();
        _textItems = new ObservableCollection<TextItemState>();
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

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(ref _topic, value);
    }
}