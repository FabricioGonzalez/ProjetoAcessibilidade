using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Input;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IEditingBodyViewModel : IEditingBody
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