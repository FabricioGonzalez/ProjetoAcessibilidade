using ProjectAvalonia.Common.Extensions;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Models.ValidationTypes;

public class ObrigatorityOperation : ReactiveObject, IOperationType
{
    public string Value
    {
        get;
    } = "Obrigatority";

    public string LocalizationKey
    {
        get;
    } = "ObrigatorityLabel".GetLocalized();
}