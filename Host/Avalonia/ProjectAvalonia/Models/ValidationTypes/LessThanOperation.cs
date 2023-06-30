using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.States.ValidationRulesState;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class LessThanOperation : ReactiveObject, ICheckingOperationType
{
    public string Value
    {
        get;
    } = "<=";

    public string LocalizationKey
    {
        get;
    } = "LessThanOperationLabel".GetLocalized();
}