using Common.Optional;
using ProjectAvalonia.Presentation.Enums;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States.FormItemState;

public class FormItemContainer : ReactiveObject
{
    private readonly Dictionary<AppFormDataType, FormItemStateBase> History = new();
    private FormItemStateBase _body;

    private string _id;

    private string _topic;

    private AppFormDataType _type;

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(backingField: ref _id, newValue: value);
    }

    public AppFormDataType Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(backingField: ref _type, newValue: value);
    }

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(backingField: ref _topic, newValue: value);
    }

    public FormItemStateBase Body
    {
        get => _body;
        set
        {
            History.TryAdd(key: _type, value: value);

            this.RaiseAndSetIfChanged(backingField: ref _body, newValue: value);
        }
    }

    public void ClearHistory() => History.Clear();

    public void AddToHistory(
        FormItemStateBase formItem
    ) => History.TryAdd(key: formItem.Type, value: formItem);

    public Optional<FormItemStateBase> ChangeItem(
        AppFormDataType type
    ) => History.GetValueOrDefault(type).ToOption();
}

public abstract class FormItemStateBase
    : ReactiveObject
{
    private string _id;
    private string _topic;

    private AppFormDataType _type;

    protected FormItemStateBase(
        AppFormDataType type
        , string id
    )
    {
        _id = id;
        Type = type;
    }

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(backingField: ref _topic, newValue: value);
    }

    public AppFormDataType Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(backingField: ref _type, newValue: value);
    }

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(backingField: ref _id, newValue: value);
    }
}