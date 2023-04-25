using System.Collections.ObjectModel;
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
    }

    public ObservableCollection<ILawListViewModel> LawList
    {
        get;
    }

    public ObservableCollection<IFormViewModel> Form
    {
        get;
    }
}