using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SystemApplication.Services;
public class NotifierBaseClass : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void NotifyPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    protected NotifierBaseClass SetAtributeValue<T>(ref T item, T value)
    {
        item = value;
        return this;
    }
}
