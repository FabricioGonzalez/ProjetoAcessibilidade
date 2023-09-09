using System.ComponentModel;
using ProjectAvalonia.Presentation.Enums;

namespace ProjectAvalonia.Models.ValidationTypes;

public interface ICheckingRuleTypes : INotifyPropertyChanged
{
    AppFormDataType Value
    {
        get;
    }

    string LocalizationKey
    {
        get;
    }
}