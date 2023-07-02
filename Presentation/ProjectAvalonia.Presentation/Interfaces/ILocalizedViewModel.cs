using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ILocalizedViewModel : INotifyPropertyChanged
{
    public string Title
    {
        get;
        set;
    }

    public string? LocalizedTitle
    {
        get;
        set;
    }
}