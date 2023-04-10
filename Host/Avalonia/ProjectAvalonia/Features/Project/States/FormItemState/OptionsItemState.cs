using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.FormItemState;

public partial class OptionsItemState : ReactiveObject
{
    [AutoNotify]
    private string _id;

    [AutoNotify]
    private bool _isChecked;

    [AutoNotify]
    private string _value;
}