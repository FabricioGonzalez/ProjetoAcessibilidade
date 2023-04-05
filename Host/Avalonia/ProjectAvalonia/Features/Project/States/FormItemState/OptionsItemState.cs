using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.FormItemState;
public partial class OptionsItemState : ReactiveObject
{
    [AutoNotify]
    private string _id;

    [AutoNotify]
    private string _value;

    [AutoNotify]
    private bool _isChecked = false;

}
