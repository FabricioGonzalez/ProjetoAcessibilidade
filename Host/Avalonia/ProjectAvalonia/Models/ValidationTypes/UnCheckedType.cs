using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class UnCheckedType
    : ReactiveObject
        , ICheckingValue
{
    public string Value
    {
        get;
        set;
    } = "unchecked";

    public string LocalizationKey
    {
        get;
    } = "UncheckedValueLabel".GetLocalized();
}