using DynamicData.Binding;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.FormItemState;

public partial class CheckboxItemState : ReactiveObject
{
    [AutoNotify]
    private string _id;

    [AutoNotify]
    private ObservableCollectionExtended<OptionsItemState> _options;

    [AutoNotify]
    private ObservableCollectionExtended<TextItemState> _textItems;

    [AutoNotify]
    private string _topic;
}