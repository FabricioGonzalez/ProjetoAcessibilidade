using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ISolutionGroupViewModel : INotifyPropertyChanged
{
    public ObservableCollection<ISolutionLocationItem> LocationItems
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