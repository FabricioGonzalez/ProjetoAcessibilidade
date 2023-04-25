using System.Collections.ObjectModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IEditingBodyViewModel
{
    public ObservableCollection<ILawListViewModel> LawList
    {
        get;
    }

    public ObservableCollection<IFormViewModel> Form
    {
        get;
    }
}