using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class IsOperation
    : ReactiveObject
        , ICheckingOperationType
{
    public string Value
    {
        get;
    } = "is";

    public string LocalizationKey
    {
        get;
    } = "IsOperationLabel".GetLocalized();
}