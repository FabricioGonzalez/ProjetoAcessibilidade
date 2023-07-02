﻿using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class CheckedType
    : ReactiveObject
        , ICheckingValue
{
    public string Value
    {
        get;
    } = "checked";

    public string LocalizationKey
    {
        get;
    } = "CheckedValueLabel".GetLocalized();
}

public class TextType
    : ReactiveObject
        , ICheckingValue
{
    private string _value;

    public TextType(
        string textType
    )
    {
        Value = textType;
    }

    public string Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public string LocalizationKey
    {
        get;
    } = "CheckedValueLabel".GetLocalized();
}