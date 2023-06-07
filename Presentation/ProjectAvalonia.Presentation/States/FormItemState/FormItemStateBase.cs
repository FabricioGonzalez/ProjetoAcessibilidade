using ProjetoAcessibilidade.Core.Enuns;

using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class FormItemContainer : ReactiveObject
{
    private FormItemStateBase _body;

    private string _id;

    private string _topic;

    private AppFormDataType _type;

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

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(ref _topic, value);
    }

    public FormItemStateBase Body
    {
        get => _body;
        set => this.RaiseAndSetIfChanged(ref _body, value);
    }
}

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