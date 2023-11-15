using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class LessThanOperation
    : ReactiveObject
        , ICheckingOperationType
{
    public string Value
    {
        get;
    } = "less_equal_than";

    public string LocalizationKey
    {
        get;
    } = "LessThanOperationLabel".GetLocalized();
}