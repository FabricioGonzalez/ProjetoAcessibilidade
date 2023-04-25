using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IProjectEditingViewModel : INotifyPropertyChanged
{
    public IEditingItemViewModel SelectedItem
    {
        get;
    }

    public ObservableCollection<IEditingItemViewModel> EditingItems
    {
        get;
    }
}