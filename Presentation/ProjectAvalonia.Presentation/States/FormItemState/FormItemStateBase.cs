using ProjetoAcessibilidade.Core.Enuns;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public abstract class FormItemStateBase : ReactiveObject
{
    private string _id;

    private AppFormDataType _type;

    protected FormItemStateBase(
        AppFormDataType type
        , string id
    )
    {
        id = id;
        Type = type;
    }

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public AppFormDataType Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(ref _type, value);
    }
}