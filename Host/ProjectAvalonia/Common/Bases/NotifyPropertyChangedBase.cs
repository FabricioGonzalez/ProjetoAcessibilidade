using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectAvalonia.Common.Bases;

public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
{
    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null
    ) => PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName: propertyName));

    protected bool RaiseAndSetIfChanged<T>(
        ref T field
        , T value
        , [CallerMemberName] string? propertyName = null
    )
    {
        if (EqualityComparer<T>.Default.Equals(x: field, y: value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName: propertyName);
        return true;
    }

    #endregion Events
}