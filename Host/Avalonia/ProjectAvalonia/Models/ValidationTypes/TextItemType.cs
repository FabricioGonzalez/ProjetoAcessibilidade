using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Enums;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class TextItemType
    : ReactiveObject
        , ICheckingRuleTypes
{
    public AppFormDataType Value
    {
        get;
    } = AppFormDataType.Text;

    public string LocalizationKey
    {
        get;
    } = "TextItemTypeLabel".GetLocalized();
}