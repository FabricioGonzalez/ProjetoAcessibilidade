using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SystemApplication.Services;
public class NotifierBaseClass : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void NotifyPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    protected void SetAtributeValue<T>(ref T item, T value, [CallerMemberName] string? name = null)
    {
        item = value;
        NotifyPropertyChanged(name);
    }
}
