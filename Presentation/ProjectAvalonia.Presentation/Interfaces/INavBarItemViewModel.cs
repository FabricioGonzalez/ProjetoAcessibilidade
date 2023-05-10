using System.ComponentModel;
using System.Reactive;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface INavBarItemViewModel
    : INotifyPropertyChanged
{
    bool IsSelected
    {
        get;
        set;
    }

    bool IsSelectable
    {
        get;
    }

    ReactiveCommand<bool, Unit> OpenCommand
    {
        get;
    }

    public string LocalizedTitle
    {
        get;
    }

    void Toggle();
}