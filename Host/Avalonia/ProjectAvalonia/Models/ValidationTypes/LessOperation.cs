using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class LessOperation
    : ReactiveObject
        , ICheckingOperationType
{
    public string Value => "lest_than";

    public string LocalizationKey
    {
        get;
    } = "LessOperationLabel".GetLocalized();
}