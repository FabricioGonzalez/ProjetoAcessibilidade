using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using Avalonia.Input;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IEditingBodyViewModel : INotifyPropertyChanged
{
    public ObservableCollection<ILawListViewModel> LawList
    {
        get;
    }

    public ObservableCollection<IFormViewModel> Form
    {
        get;
    }

    public KeyGesture Gesture
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }
}