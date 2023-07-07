﻿using Common.Optional;
using ProjetoAcessibilidade.Core.Enuns;
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
        set
        {
            History.TryAdd(_type, value);

            this.RaiseAndSetIfChanged(ref _body, value);
        }
    }

    public void ClearHistory() => History.Clear();

    public void AddToHistory(
        FormItemStateBase formItem
    ) => History.TryAdd(formItem.Type, formItem);

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
        set => this.RaiseAndSetIfChanged(ref _topic, value);
    }

    public AppFormDataType Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(ref _type, value);
    }

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }
}