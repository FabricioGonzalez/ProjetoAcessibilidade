using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IOperationType : INotifyPropertyChanged
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