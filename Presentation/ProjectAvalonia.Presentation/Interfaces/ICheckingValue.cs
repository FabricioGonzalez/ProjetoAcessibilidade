using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ICheckingValue : INotifyPropertyChanged
{
    string Value
    {
        get;
    }

    string LocalizationKey
    {
        get;
    }
}