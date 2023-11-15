using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Enums;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class CheckboxItemType
    : ReactiveObject
        , ICheckingRuleTypes
{
    public AppFormDataType Value
    {
        get;
    } = AppFormDataType.Checkbox;

    public string LocalizationKey
    {
        get;
    } = "CheckboxItemTypeLabel".GetLocalized();
}