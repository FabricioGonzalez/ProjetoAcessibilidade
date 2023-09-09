using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ICheckingOperationType : INotifyPropertyChanged
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