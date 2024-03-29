﻿using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

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
        set => this.RaiseAndSetIfChanged(backingField: ref _value, newValue: value);
    }

    public string LocalizationKey
    {
        get;
    } = "CheckedValueLabel".GetLocalized();
}