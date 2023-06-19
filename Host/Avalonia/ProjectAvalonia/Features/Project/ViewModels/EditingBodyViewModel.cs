using System.Collections.ObjectModel;
using System.Reactive;

using Avalonia.Input;

using ProjectAvalonia.Logging;
using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class EditingBodyViewModel : ReactiveObject, IEditingBodyViewModel
{
    public EditingBodyViewModel(ObservableCollection<ILawListViewModel> lawList,
        ObservableCollection<IFormViewModel> form)
    {
        LawList = lawList;
        Form = form;

        SaveItemCommand = ReactiveCommand.Create(execute: () =>
        {
            Logger.LogDebug("Ok, saving");

        });
    }

    public ObservableCollection<ILawListViewModel> LawList
    {
        get;
    }
    public KeyGesture Gesture
    {
        get;
    } = new KeyGesture(Key.S, KeyModifiers.Control);
    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }

    public ObservableCollection<IFormViewModel> Form
    {
        get;
    }
}