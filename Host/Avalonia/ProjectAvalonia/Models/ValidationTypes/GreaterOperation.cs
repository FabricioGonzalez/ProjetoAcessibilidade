using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class GreaterOperation
    : ReactiveObject
        , ICheckingOperationType
{
    public string Value
    {
        get;
    } = ">";

    public string LocalizationKey
    {
        get;
    } = "GreaterOperationLabel".GetLocalized();
}