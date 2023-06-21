using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IOptionViewModel : INotifyPropertyChanged
{
    public string Value
    {
        get;
    }
    public string Id
    {
        get;
        set;
    }
    public bool IsChecked
    {
        get; set;
    }
}