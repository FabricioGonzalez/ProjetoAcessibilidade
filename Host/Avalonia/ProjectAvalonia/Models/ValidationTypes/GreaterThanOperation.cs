﻿using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.States.ValidationRulesState;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class GreaterThanOperation : ReactiveObject, ICheckingOperationType
{
    public string Value
    {
        get;
    } = ">=";

    public string LocalizationKey
    {
        get;
    } = "GreaterThanOperationLabel".GetLocalized();
}