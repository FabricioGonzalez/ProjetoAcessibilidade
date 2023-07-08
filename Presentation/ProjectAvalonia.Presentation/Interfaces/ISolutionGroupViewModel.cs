using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ISolutionGroupViewModel : INotifyPropertyChanged
{
    public ObservableCollection<IItemGroupViewModel> ItemsGroups
    {
        get;
        set;
    }

    public IItemViewModel ConclusionItem
    {
        get;
        set;
    }

    public IItemViewModel SolutionItem
    {
        get;
        set;
    }
}