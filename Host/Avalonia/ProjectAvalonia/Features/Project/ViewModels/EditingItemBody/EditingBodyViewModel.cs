using System.Collections.ObjectModel;
using System.Reactive;
using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels.EditingItemBody;

public class EditingBodyViewModel
    : ReactiveObject
        , IEditingBodyViewModel
{
    public EditingBodyViewModel(
        ObservableCollection<ILawListViewModel> lawList
        , ObservableCollection<IFormViewModel> form
    )
    {
        LawList = lawList;
        Form = form;


        SaveItemCommand = ReactiveCommand.Create(() =>
        {
            Logger.LogDebug("Ok, saving");
        });
    }

    public ObservableCollection<ILawListViewModel> LawList
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }

    public ObservableCollection<IFormViewModel> Form
    {
        get;
    }
}