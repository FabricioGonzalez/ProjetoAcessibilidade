using DynamicData.Binding;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.FormItemState;
public class CheckboxItemState : ReactiveObject
{
    private string topic;
    public string Topic
    {
        get => topic;
        set => this.RaiseAndSetIfChanged(ref topic, value);
    }

    private ObservableCollectionExtended<OptionsItemState> options;
    public ObservableCollectionExtended<OptionsItemState> Options
    {
        get => options;
        set => this.RaiseAndSetIfChanged(ref options, value);
    }

    private ObservableCollectionExtended<TextItemState> textItems;
    public ObservableCollectionExtended<TextItemState> TextItems
    {
        get => textItems;
        set => this.RaiseAndSetIfChanged(ref textItems, value);
    }
}
