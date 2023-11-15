using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class GreaterThanOperation
    : ReactiveObject
        , ICheckingOperationType
{
    public string Value
    {
        get;
    } = "greater_equal_than";

    public string LocalizationKey
    {
        get;
    } = "GreaterThanOperationLabel".GetLocalized();
}