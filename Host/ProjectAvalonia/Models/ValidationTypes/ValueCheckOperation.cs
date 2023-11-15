using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class ValueCheckOperation : ReactiveObject, IOperationType
{
    public string Value
    {
        get;
    } = "ValueCheck";

    public string LocalizationKey
    {
        get;
    } = "ValueCheckLabel".GetLocalized();
}