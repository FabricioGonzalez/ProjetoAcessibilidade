using ProjectAvalonia.Common.Extensions;
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
        set;
    } = "checked";

    public string LocalizationKey
    {
        get;
    } = "CheckedValueLabel".GetLocalized();
}