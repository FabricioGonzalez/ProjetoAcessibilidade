using Core.Enuns;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.States.FormItemState;

public abstract partial class FormItemStateBase : ReactiveObject
{
    [AutoNotify]
    private string _id;

    [AutoNotify]
    private AppFormDataType _type;

    public FormItemStateBase(
        AppFormDataType type
        , string id
    )
    {
        id = id;
        Type = type;
    }
}