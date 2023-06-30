﻿using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.States.ValidationRulesState;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class HasOperation : ReactiveObject, ICheckingOperationType
{
    public string Value
    {
        get;
    } = "has";

    public string LocalizationKey
    {
        get;
    } = "HasOperationLabel".GetLocalized();
}