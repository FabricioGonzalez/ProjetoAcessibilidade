using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ICheckingValue : INotifyPropertyChanged
{
    string Value
    {
        get;
        set;
    }

    string LocalizationKey
    {
        get;
    }
}